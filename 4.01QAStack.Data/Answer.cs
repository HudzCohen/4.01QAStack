using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4._01QAStack.Data
{
    public class Answer
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int QuestionId { get; set; }
        public DateTime Date { get; set; }
        public string AnswerText { get; set; }


        public User User { get; set; }
        public Question Question { get; set; }

    }
}
