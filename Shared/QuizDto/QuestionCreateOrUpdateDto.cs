using Domain.Models.subject_Lesson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Shared.QuizDto
{
    public class QuestionCreateOrUpdateDto
    {
        [Required]
        public  string QuestionText { get; set; }
        public QuestionType? QuestionType { get; set; } = QuizDto.QuestionType.MultipleChoice;
        public int? Points { get; set; } = 1;  // default 1 point


    }
}
