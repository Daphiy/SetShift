using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using SetShifting.Controllers;

namespace SetShifting.Models
{
    public class ModelManager : MonoBehaviour
    {
        public static ModelManager Instance;

        List<Question> m_questions;
        List<Question> m_fquestions;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            JSONNode questionsNode = JSON.Parse(QuestionnaireController.Instance.QData.ToString()); 
            m_questions = ModelParser.Instance.ParseAllQuestions(questionsNode);
            JSONNode fquestionsNode = JSON.Parse(QuestionnaireController.Instance.FQData.ToString()); 
            m_fquestions = ModelParser.Instance.ParseAllQuestions(fquestionsNode);
        }

        public List<Question> Questions { get => m_questions; set => m_questions = value; }
        public List<Question> FQuestions { get => m_fquestions; set => m_fquestions = value; }
    }
}