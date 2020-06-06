using SetShifting.Controllers;
using SetShifting.Models;
using System.Collections;
using TMPro;
using UnityEngine;

namespace SetShifting.Views
{
    public class QuestionView : MonoBehaviour
    {
        [SerializeField] TMP_Text m_questionText;
        [SerializeField] TMP_Text m_answer1;
        [SerializeField] TMP_Text m_answer2;
        [SerializeField] TMP_Text m_answer3;
        [SerializeField] TMP_Text m_answer4;
        [SerializeField] TMP_Text m_answer5;
        [SerializeField] TMP_Text m_answer6;
        [SerializeField] TMP_Text m_answer7;
        [SerializeField] GameObject m_validationMessage;

        Question m_question;
        public void SetQuestion(Question question)
        {
            m_question = question;
            m_questionText.text = m_question.Q;
            m_answer1.text = m_question.Answer1;
            m_answer2.text = m_question.Answer2;
            m_answer3.text = m_question.Answer3;
            
            if (m_answer4 != null)
                m_answer4.text = m_question.Answer4;
            if (m_answer5 != null)
                m_answer5.text = m_question.Answer5;

            if (m_answer6 != null)
                m_answer6.text = m_question.Answer6;

            if (m_answer7 != null)
                m_answer7.text = m_question.Answer7;
            m_question.UsersAnswer = -1;
        }

        public void Button_NextQuestion()
        {
            QuestionnaireController.Instance.NextQuestion();
        }
        
        public void Button_NextQuestionFirstTime()
        {
            QuestionnaireController.Instance.NextQuestionFirstTime();
        }

        public void Button_selectedAnwser(int answerNumber)
        {
            if (m_question == null)
                return;

            m_question.UsersAnswer = answerNumber;
        }

        public IEnumerator Show_validation()
        {
            m_validationMessage.SetActive(true);
            yield return new WaitForSecondsRealtime(0.6f);
            m_validationMessage.SetActive(false);
        }
    }
}
