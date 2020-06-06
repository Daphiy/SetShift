using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SimpleJSON;
using SetShifting.Models;
using SetShifting.controllers;
using System;

namespace SetShifting.Controllers
{
    public class GameManager : MonoBehaviour
    {
        int total_rounds;
        bool isWritten = false;

        [SerializeField] GameObject m_gameContainer_wisconsin;
        [SerializeField] GameObject m_gameContainer_twist;
        float m_timer;

        List<string> m_deckCards_A = new List<string>();
        List<string> m_deckCards_Bright = new List<string>();
        List<string> m_deckCards_Bleft = new List<string>();
        List<string> m_deckCards_C = new List<string>();
        int m_currentRound = 0;
        int PROB_START_ROUND = 33;

        List<int> m_answers_A = new List<int>();
        List<int> m_answers_B = new List<int>();
        List<int> m_answers_C = new List<int>();
        [SerializeField] TMP_Text status;
        [SerializeField] TMP_Text answerText;
        [SerializeField] GameObject status_popup;
        [SerializeField] GameObject end_game_popup;
        [SerializeField] GameObject info_popup;
        [SerializeField] GameObject end_phaseA_popup;
        [SerializeField] GameObject end_phaseA_visual_popup;
        [SerializeField] GameObject end_phaseB_popup;
        [SerializeField] GameObject end_phaseB_visual_popup;
        [SerializeField] List<Sprite> sprites;
        [SerializeField] Image topDeckCard;

        [SerializeField] Image cardLeft;
        [SerializeField] Image cardRight;

        [SerializeField] TextAsset m_trials_phaseA;
        [SerializeField] TextAsset m_trials_phaseB;
        [SerializeField] TextAsset m_trials_phaseC;

        [SerializeField] GameObject m_twistRules;
        [SerializeField] GameObject m_wisconsinRules;
        [SerializeField] GameObject m_twistProbRules; 

        /// Do NOT change this dict !! 
        [SerializeField] Serialized_KeyStringValueSprite_Dictionary m_spriteDict;
        // 

        // in the header
        [SerializeField] TMP_Text m_title;
        Color32 m_textColorRegular = new Color32(168, 174, 178, 255);
        Color32 m_textColorSelected = new Color32(255, 255, 255, 255);

        // Add a Component to the text "EventTrigger" add the "PointerEnter" and "PointerExit" and connect them the the public functions:
        // OnMouseExit
        //m_title.color = m_textColorRegular;

        // OnMouseOver
        //m_title.color = m_textColorSelected;

        public static GameManager Instance;
        public enum PhasesEnum
        {
            A = 0, B, C
        };
        PhasesEnum m_phase = PhasesEnum.A;
        private Dictionary<PhasesEnum, int> m_phaseDict = new Dictionary<PhasesEnum, int>();
        void Awake()
        {
            Instance = this;
            m_gameContainer_wisconsin.SetActive(false);
            m_gameContainer_twist.SetActive(false);
        }
        void Update()
        {
            m_timer += Time.deltaTime;

            if (m_phase != PhasesEnum.B) return;
            if (m_currentRound >= total_rounds) return;

            else if (Input.GetKeyDown(KeyCode.F)) Key_Selection(0);
            if (Input.GetKeyDown(KeyCode.A)) Key_Selection(1);
        }
        void Start()
        {
            Screen.fullScreen = true;
            QuestionnaireController.Instance.StartQuestionnaire();

            (m_deckCards_A, m_answers_A) = ModelParser.Instance.ParseTrials(JSON.Parse(m_trials_phaseA.ToString()));
            (m_deckCards_Bleft, m_deckCards_Bright, m_answers_B) = ModelParser.Instance.ParseTwistTrials(JSON.Parse(m_trials_phaseB.ToString()));
            (m_deckCards_C, m_answers_C) = ModelParser.Instance.ParseTrials(JSON.Parse(m_trials_phaseC.ToString()));
        }
        public void StartTheGame()
        {   

            m_currentRound = 0;
            m_timer = 0;
            string sprite_id = null;
            string sprite_idL = null;
            string sprite_idR = null;
            answerText.text = "";

            switch (m_phase)
            {
                case PhasesEnum.A:
                    m_gameContainer_wisconsin.SetActive(true);
                    sprite_id = m_deckCards_A[m_currentRound];
                    topDeckCard.sprite = m_spriteDict[sprite_id];
                    total_rounds = m_deckCards_A.Count;
                    break;

                case PhasesEnum.B:
                    sprite_idL = m_deckCards_Bleft[m_currentRound];
                    sprite_idR = m_deckCards_Bright[m_currentRound];
                    cardLeft.sprite = m_spriteDict[sprite_idL];
                    cardRight.sprite = m_spriteDict[sprite_idR];
                    total_rounds = m_deckCards_Bleft.Count;
                    break;

                case PhasesEnum.C:
                    m_gameContainer_wisconsin.SetActive(true);
                    sprite_id = m_deckCards_C[m_currentRound];
                    topDeckCard.sprite = m_spriteDict[sprite_id];
                    total_rounds = m_deckCards_C.Count;
                    break;
            }
            status_popup.SetActive(false);
            // CHANGE ME
            end_game_popup.SetActive(false); 
        }

        public void to_visual()
        {
            switch (m_phase)
            // Goes from phase A to B, or from phase B to C
            {
                case PhasesEnum.B:
                    end_phaseB_popup.SetActive(false);
                    end_phaseB_visual_popup.SetActive(true); 
                    break;
                case PhasesEnum.A:
                    end_phaseA_popup.SetActive(false);
                    end_phaseA_visual_popup.SetActive(true);
                    break;
            }
        }
        public void StartNextPhase()
        {
            switch (m_phase)
            // Goes from phase A to B, or from phase B to C
            {
                case PhasesEnum.B:
                    m_phase = PhasesEnum.C;
                    end_phaseB_visual_popup.SetActive(false);
                    m_gameContainer_twist.SetActive(false);
                    m_gameContainer_wisconsin.SetActive(true);
                    break;
                case PhasesEnum.A:
                    m_phase = PhasesEnum.B;
                    end_phaseA_visual_popup.SetActive(false);
                    m_gameContainer_wisconsin.SetActive(false);
                    m_gameContainer_twist.SetActive(true);
                    break;
            }
            StartTheGame();
        }
        void EndPhase()
        {
            switch (m_phase)
            {
                case PhasesEnum.A:
                    end_phaseA_popup.SetActive(true);
                    m_gameContainer_wisconsin.SetActive(false);
                    break;
                case PhasesEnum.B:
                    end_phaseB_popup.SetActive(true);
                    m_gameContainer_twist.SetActive(false);
                    break;
                case PhasesEnum.C:
                    end_game_popup.SetActive(true);
                    m_gameContainer_wisconsin.SetActive(false);
                    break;
            }
        }
        // Go to next round 
        void StartNextRound()
        {
            m_currentRound++;
            m_timer = 0;
            isWritten = false;
            StopAllCoroutines();

            string sprite_id = null;
            string sprite_idL = null;
            string sprite_idR = null;

            if (m_currentRound == total_rounds)
            {
                EndPhase();
                return;
            }

            if (m_phase == PhasesEnum.A)
            {
                sprite_id = m_deckCards_A[m_currentRound];
                topDeckCard.sprite = m_spriteDict[sprite_id];
                //answerText.text = m_answers_A[m_currentRound].ToString();
            }
            if (m_phase == PhasesEnum.B)
            {
                sprite_idL = m_deckCards_Bleft[m_currentRound];
                sprite_idR = m_deckCards_Bright[m_currentRound];

                cardLeft.sprite = m_spriteDict[sprite_idL];
                cardRight.sprite = m_spriteDict[sprite_idR];
                //answerText.text = m_answers_B[m_currentRound].ToString();
                if ( m_currentRound == PROB_START_ROUND)
                {
                    m_twistProbRules.SetActive(true); 
                }
            }
            else if (m_phase == PhasesEnum.C)
            {
                sprite_id = m_deckCards_C[m_currentRound];
                topDeckCard.sprite = m_spriteDict[sprite_id];
                //answerText.text = m_answers_C[m_currentRound].ToString();
            }
        }

        public void TwistRules()
        {
            m_twistRules.SetActive(true);
        }
        public void CloseTwistRules()
        {
            m_twistRules.SetActive(false);
        }

        public void CloseTwistProbRules()
        {
            m_twistProbRules.SetActive(false);
        }
        public void WisconsinRules()
        {
            m_wisconsinRules.SetActive(true);
        }
        public void CloseWisconsinRules()
        {
            m_wisconsinRules.SetActive(false);
        }

        public void InfoPopup()
        {
            end_game_popup.SetActive(false);
            info_popup.SetActive(true); 
        }

        public void InfoPopupBack()
        {
            end_game_popup.SetActive(true);
            info_popup.SetActive(false);
        }

        IEnumerator NextRound()
        {
            yield return new WaitForSeconds(0.6f);
            status_popup.SetActive(false);
            StartNextRound();
        }
        // User Input Handling 
        public void Button_Select_Card(int cardNum)
        {
            Key_Selection(cardNum);
        }
        public void Key_Selection(int selection)
        {
            //write number into table 
            if (!isWritten)
            {
                RemoteLogger.instance.SetCardData(m_phase.ToString(), m_currentRound, selection, m_timer);
                isWritten = true;
            }

            status_popup.SetActive(true);

            if (m_phase == PhasesEnum.A)
                status.text = (selection == m_answers_A[m_currentRound]) ? "correct<sprite=13>" : "wrong<sprite=12>";
            else if (m_phase == PhasesEnum.B)
                status.text = (selection == m_answers_B[m_currentRound]) ? "correct<sprite=13>" : "wrong<sprite=12>";
            else if (m_phase == PhasesEnum.C)
                status.text = (selection == m_answers_C[m_currentRound]) ? "correct<sprite=13>" : "wrong<sprite=12>";
            StartCoroutine(NextRound());
        }
        //
        public PhasesEnum Phase { get => m_phase; set => m_phase = value; }

        [Serializable]
        public class Serialized_KeyStringValueSprite_Dictionary : SerializableDictionary<string, Sprite>
        {

        }

       
    }
}
