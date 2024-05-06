namespace _4._01QAStack.Data
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public int Likes { get; set; }

        public List<QuestionTag> QuestionTags { get; set; } 
        public List<Answer> Answers { get; set; }
        public User User { get; set; }
    }
}
