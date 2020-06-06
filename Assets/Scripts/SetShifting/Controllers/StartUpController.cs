using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SetShifting.Controllers
{
    public class StartUpController : MonoBehaviour
    {
        [SerializeField] GameObject m_intro;
        [SerializeField] GameObject m_questionnaire;
        bool inQuestionnaireIntro = false;
        private void Awake()
        {
            Screen.fullScreen = true;
            m_intro.SetActive(true);
            m_questionnaire.SetActive(false);
        }
        public void Button_ToTheIntro()
        {
            m_intro.SetActive(false);
            m_questionnaire.SetActive(true);
            inQuestionnaireIntro = true; 
        }
        public void Button_NextScene()
        {
            inQuestionnaireIntro = false; 
            SceneManager.LoadScene(1);
        }
        void Update()
        {
            if (!inQuestionnaireIntro)
                return; 
            if (Input.GetKeyDown(KeyCode.Return)) Button_NextScene();
        }
    }
}
