using Domain.Models.subject_Lesson;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.QuizDto
{
    public class QuizDto
    {
        public string QuizCode { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TotalMarks { get; set; }
       
        //Foreign Key
        public int LessonId { get; set; }
        // Navigation property
        public ICollection<QuestionDto>? Questions { get; set; }
        

    }
}
