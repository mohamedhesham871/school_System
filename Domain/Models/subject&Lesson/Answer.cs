namespace Domain.Models.subject_Lesson
{
    public class Answer : IHasCode
    {
        public int Id { get; set; }
        // Code Answer
        public string Code { set; get; }

        public required string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
       
        // Foreign key & Navigation property
        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public Answer()
        {
            Code = Guid.NewGuid().ToString();
        }
    }
}