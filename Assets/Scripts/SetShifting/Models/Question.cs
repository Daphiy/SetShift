namespace SetShifting.Models
{
    public class Question
    {
        int m_id;
        string m_question;
        string m_answer1;
        string m_answer2;
        string m_answer3;
        string m_answer4;
        string m_answer5;
        string m_answer6;
        string m_answer7;

        int usersAnswer = -1;
        public Question(int id, string question, string answer1, string answer2, string answer3, string answer4)
        {
            m_id = id;
            m_question = question;
            m_answer1 = answer1;
            m_answer2 = answer2;
            m_answer3 = answer3;
            m_answer4 = answer4;
        }
        public Question(int id, string question, string answer1, string answer2, string answer3, string answer4, string answer5, string answer6, string answer7)
        {
            m_id = id;
            m_question = question;
            m_answer1 = answer1;
            m_answer2 = answer2;
            m_answer3 = answer3;
            m_answer4 = answer4;
            m_answer5 = answer5;
            m_answer6 = answer6;
            m_answer7 = answer7;
        }
        public int Id { get => m_id; set => m_id = value; }
        public string Q { get => m_question; set => m_question = value; }
        public string Answer1 { get => m_answer1; set => m_answer1 = value; }
        public string Answer2 { get => m_answer2; set => m_answer2 = value; }
        public string Answer3 { get => m_answer3; set => m_answer3 = value; }
        public string Answer4 { get => m_answer4; set => m_answer4 = value; }
        public string Answer5 { get => m_answer5; set => m_answer5 = value; }
        public string Answer6 { get => m_answer6; set => m_answer6 = value; }
        public string Answer7 { get => m_answer7; set => m_answer7 = value; }
        public int UsersAnswer { get => usersAnswer; set => usersAnswer = value; }
    }
}
