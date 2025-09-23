using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.subject_Lesson
{
    public class Quiz:IHasCode
    {
        public int QuizId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TotalMarks { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; } 

        //Foreign Key
        public int LessonId { get; set; }   
        // Navigation property
        public ICollection<Question>? Questions { get; set; }
        public ICollection<QuizStudent>? QuizStudents { get; set; } 
        public Lesson Lesson { get; set; } = null!;  // Navigation to Lesson


        public Quiz()
        {
            Questions = new List<Question>();
            QuizStudents = new List<QuizStudent>();
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Code = Guid.NewGuid().ToString();
            IsActive = false;
        }
    }
}
