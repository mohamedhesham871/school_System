using Domain.Models.subject_Lesson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.QuizDto
{
    public class QuestionDto
    {
        public string QuestionCode { get; set; } 
        public required string QuestionText { get; set; }
        public required string QuestionType { get; set; }//  it's string in database but enum when used in code

        public int? Points { get; set; } = 1; // default 1 point
        public List<AnswerDtoResponse>? Answers { get; set; }
        
          public Quiz Quiz { get; set; }

       
    }
}
