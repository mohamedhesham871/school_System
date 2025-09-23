using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.subject_Lesson
{
    public class Question:IHasCode
    {
        public int QuestionId { get; set; }
        public string Code { get; set; } // public unique identifier

        public required string QuestionText { get; set; }
        public required string QuestionType { get; set; }//  it's string in database but enum when used in code

        public int? Points { get; set; }  // default 1 point
        public ICollection<Answer>? Answers { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        // Foreign  & Navigation property

        public int QuizId { get; set; }//fk
        public Quiz Quiz { get; set; }

        public Question()
        {
             Code = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Points = 1;
        }
    }
}
