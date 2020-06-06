using SetShifting.Models;
using SetShifting.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SetShifting.Controllers.GameManager;

namespace SetShifting.Controllers
{
    public class QuestionnaireController : MonoBehaviour
    {
        public static QuestionnaireController Instance;
        [SerializeField] TMP_Text m_qAmount;
        [SerializeField] TextAsset m_qData;
        [SerializeField] TextAsset m_fqData;
        [SerializeField] GameObject m_questionPrefab;
        [SerializeField] GameObject m_fQuestionPrefab;
        [SerializeField] GameObject m_genderQuestionPrefab;
        [SerializeField] GameObject m_ageQuestionPrefab;
        [SerializeField] GameObject m_educationQuestionPrefab;
        [SerializeField] Transform m_questionContainer;
        [SerializeField] Transform m_firstQuestionContainer;
        [SerializeField] GameObject m_questionnaireMessage;
        [SerializeField] GameObject m_gameRules;
        [SerializeField] GameObject m_questionSegment;
        [SerializeField] GameObject m_visual_rules_popup;
        [SerializeField] GameObject m_feedback_button;
        [SerializeField] Button m_button;

        float m_timer;

        bool m_inFirst = true;
        bool inQ = true; //tells if we are still in the questionnaire phase 
        bool m_inFeedbackQ = false; 
        
        int m_currentQuestion = -1;
        int m_numNonq = 3; //represents num of non-questionnaire questions 
        
        bool is_suicidal = false;
        int m_suicide_question_num = 11;
        
        List<GameObject> questions = new List<GameObject>();

        public Action AnswerWasChosen;
        public TextAsset QData { get => m_qData; set => m_qData = value; }
        public TextAsset FQData { get => m_fqData; set => m_fqData = value; }

        void Awake()
        {
            Instance = this;
        }
        private void Update()
        {
            if (!inQ) return; 
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (m_inFirst)
                    NextQuestionFirstTime();
                else
                    NextQuestion();
            }
            m_timer += Time.deltaTime;
        }
        public void StartQuestionnaire()
        {
            m_questionSegment.SetActive(true);

            // make sure pop-ups are off 
            m_questionnaireMessage.SetActive(false);
            m_gameRules.SetActive(false);
            m_visual_rules_popup.SetActive(false);

            m_currentQuestion = 0;
            m_timer = 0;
            m_qAmount.text = "";
  
            SetGenderQuestion(ModelManager.Instance.Questions[0]);
            SetAgeQuestion(ModelManager.Instance.Questions[1]);
            SetEducationQuestion(ModelManager.Instance.Questions[2]);
        }
        public void StartFQuestionnaire()
        {
            m_feedback_button.SetActive(false); 
            m_qAmount.gameObject.SetActive(true);
            m_currentQuestion = 0;
            setfqAmountText(); 
            m_questionSegment.SetActive(true);
            SetFQuestion(ModelManager.Instance.FQuestions[m_currentQuestion]);
            m_inFeedbackQ = true;  
            inQ = true;
        }

        public void SetGenderQuestion(Question question)
        {
            GameObject questionObject = Instantiate(m_genderQuestionPrefab, m_firstQuestionContainer);
            questionObject.GetComponent<QuestionView>().SetQuestion(question);
            questions.Add(questionObject);
        }
        public void SetAgeQuestion(Question question)
        {
            GameObject questionObject = Instantiate(m_ageQuestionPrefab, m_firstQuestionContainer);
            questionObject.GetComponent<QuestionView>().SetQuestion(question);
            questions.Add(questionObject);
        }
        public void SetEducationQuestion(Question question)
        {
            GameObject questionObject = Instantiate(m_educationQuestionPrefab, m_firstQuestionContainer);
            questionObject.GetComponent<QuestionView>().SetQuestion(question);
            questions.Add(questionObject);
        }
        public void SetQuestion(Question question)
        {
            GameObject questionObject = Instantiate(m_questionPrefab, m_questionContainer);
            questionObject.GetComponent<QuestionView>().SetQuestion(question);
            questions.Add(questionObject);
        }
        public void SetFQuestion(Question question)
        {
            GameObject questionObject = Instantiate(m_fQuestionPrefab, m_questionContainer);
            questionObject.GetComponent<QuestionView>().SetQuestion(question);
            questions.Add(questionObject);
        }
        public void NextQuestionFirstTime()
        {
            int userAnswer0 = ModelManager.Instance.Questions[0].UsersAnswer;
            int userAnswer1 = ModelManager.Instance.Questions[1].UsersAnswer;
            int userAnswer2 = ModelManager.Instance.Questions[2].UsersAnswer;

            if (userAnswer0 == -1 || userAnswer1 == -1 || userAnswer2 == -1)
            {
                // error message : "You need to answer the question first" 
                StartCoroutine(questions[0].GetComponent<QuestionView>().Show_validation());
                return;
            }

            // send data to DB
            RemoteLogger.instance.SetQuestionnaireData(0, userAnswer0, m_timer);
            RemoteLogger.instance.SetQuestionnaireData(1, userAnswer1, m_timer);
            RemoteLogger.instance.SetQuestionnaireData(2, userAnswer2, m_timer);
            //

            for (int k = 0; k < m_numNonq; k++)
                questions[m_currentQuestion + k].SetActive(false);
                
            m_inFirst = false;
            m_currentQuestion += m_numNonq;
            m_timer = 0; 
            setqAmountText();
            SetQuestion(ModelManager.Instance.Questions[m_currentQuestion]);
        }
        public void NextQuestion()
        {   
            if (m_inFeedbackQ)
            {
                NextFeedbackQuestion();
                return; 
            }

            int userAnswer = ModelManager.Instance.Questions[m_currentQuestion].UsersAnswer;

            if (userAnswer == -1)
            {
                StartCoroutine(questions[m_currentQuestion].GetComponent<QuestionView>().Show_validation());
                // error message : "You need to answer the question first" 
                return; 
            }
            // send data to DB
            RemoteLogger.instance.SetQuestionnaireData(m_currentQuestion, userAnswer, m_timer);
            
            update_suicidal(m_currentQuestion, userAnswer); 
            questions[m_currentQuestion].SetActive(false);
            m_currentQuestion++;
            m_timer = 0;

            setqAmountText();

            if (m_currentQuestion == ModelManager.Instance.Questions.Count)
            {
                // No more questions
                inQ = false;
                m_button.interactable = false;
                // open next screen 
                if (is_suicidal) // message about help centers if suicidal 
                    m_questionnaireMessage.SetActive(true);
                else 
                    go_to_game_rules(); 
                return;
            }
            SetQuestion(ModelManager.Instance.Questions[m_currentQuestion]);
        }
        public void NextFeedbackQuestion()
        {
            int userAnswer = ModelManager.Instance.FQuestions[m_currentQuestion].UsersAnswer;
            // send data to DB
            RemoteLogger.instance.SetFeedbackData(m_currentQuestion, userAnswer, m_timer);

            questions[ModelManager.Instance.Questions.Count + m_currentQuestion].SetActive(false);
            m_currentQuestion++;
            m_timer = 0;

            setfqAmountText();

            if (m_currentQuestion == ModelManager.Instance.FQuestions.Count)
            {
                // No more feedback questions
                inQ = false;
                m_button.interactable = false;
                // open end game screen 
                m_questionSegment.SetActive(false);
                GameManager.Instance.InfoPopupBack(); 
                return;
            }
            SetFQuestion(ModelManager.Instance.FQuestions[m_currentQuestion]);
        }
        public void update_suicidal(int m_currentQuestion, int userAnswer)
        {
            if (m_currentQuestion != m_suicide_question_num)
                return;
            is_suicidal = (userAnswer > 0);
            return; 
        }
        IEnumerator StartCountDown()
        {
            yield return new WaitForSecondsRealtime(5);
            m_button.interactable = true; 
        }
        public void setqAmountText()
        {
            m_qAmount.text = ((m_currentQuestion + 1) - m_numNonq) + " / " + (ModelManager.Instance.Questions.Count - m_numNonq);
        }

        public void setfqAmountText()
        {
            m_qAmount.text = (m_currentQuestion + 1) + " / " + (ModelManager.Instance.FQuestions.Count);
        }

        public void go_to_game_rules()
        {
            m_questionnaireMessage.SetActive(false);
            m_gameRules.SetActive(true);
            StartCoroutine(StartCountDown());
        }

        public void go_to_visual_rules()
        {
            m_gameRules.SetActive(false);
            m_visual_rules_popup.SetActive(true); 
        }

        public void StartGame()
        {
            m_questionSegment.SetActive(false);
            m_visual_rules_popup.SetActive(false); 
            m_qAmount.gameObject.SetActive(false);
            GameManager.Instance.StartTheGame();
        }

        public void openSuicideHotlines()
        {
            Application.OpenURL(""); 
        }
    }
}
