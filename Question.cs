using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamHelper
{
    internal class Question
    {
        public string Text { get; set; }
        public string Answer { get; set; }
        public int Difficulty { get; set; }

        public Question(string text, string answer, int difficulty)
        {
            Text = text;
            Answer = answer;
            Difficulty = difficulty;
        }
    }
}
