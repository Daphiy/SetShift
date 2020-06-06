using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

namespace SetShifting.Models
{
    public class ModelParser : MonoBehaviour
    {
        public static ModelParser Instance;

        void Awake()
        {
            Instance = this;
        }

        public (List<string>, List<string>, List<int>) ParseTwistTrials(JSONNode node)
        {
            List<string> deckCardsLeft = new List<string>();
            List<string> deckCardsRight = new List<string>();
            List<int> answers = new List<int>();

            JSONArray deckCardsLeftArray = node["Trials"]["cardsLeft"].AsArray;
            foreach (JSONNode item in deckCardsLeftArray)
                deckCardsLeft.Add(item);

            JSONArray deckCardsRightArray = node["Trials"]["cardsRight"].AsArray;
            foreach (JSONNode item in deckCardsRightArray)
                deckCardsRight.Add(item);

            JSONArray answersArray = node["Trials"]["answers"].AsArray;

            foreach (JSONNode item in answersArray)
                answers.Add(item.AsInt);

            return (deckCardsLeft, deckCardsRight, answers);
        }

        public (List<string>, List<int>) ParseTrials(JSONNode node)
        {
            List<string> deckCards = new List<string>();
            List<int> answers = new List<int>();
            JSONArray deckCardsArray = node["Trials"]["cardsDeck"].AsArray;

            foreach (JSONNode item in deckCardsArray)
                deckCards.Add(item);

            JSONArray answersArray = node["Trials"]["answers"].AsArray;

            foreach (JSONNode item in answersArray)
                answers.Add((item.AsInt - 1));

            return (deckCards, answers);
        }

        public Question ParseOneQuestion(JSONNode node)
        {
            int id = node["id"].AsInt;
            string question = node["question"];
            string answer1 = node["answer1"];
            string answer2 = node["answer2"];
            string answer3 = node["answer3"];
            string answer4 = node["answer4"];
            string answer5 = node["answer5"];
            string answer6 = node["answer6"];
            string answer7 = node["answer7"];
            if (string.IsNullOrEmpty(answer5) && string.IsNullOrEmpty(answer5))
                return new Question(id, question, answer1, answer2, answer3, answer4);
            return new Question(id, question, answer1, answer2, answer3, answer4, answer5, answer6, answer7);
        }

        public List<Question> ParseAllQuestions(JSONNode node)
        {
            List<Question> result = new List<Question>();
            JSONArray array = node["Questions"].AsArray;

            foreach (JSONNode item in array)
                result.Add(ParseOneQuestion(item));

            return result;
        }
    }
}
