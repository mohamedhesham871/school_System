using Domain.Models.subject_Lesson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.QuizDto
{
    public  class AnswerDtoResponse
    {
        public string AnswerCode { set; get; }

        public  string AnswerText { get; set; }
        public bool IsCorrect { get; set; }      
    }
}
