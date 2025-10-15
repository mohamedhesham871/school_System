using Domain.Models.subject_Lesson;
using Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.QuizDto
{
    public class QuizStudentResultDetailsForSubjectDto
    {
       
       public string StudentName { get; set; } = null!;
       public string QuizTitle { get; set; } = null!;
       public string LessonTitle { get; set; } = null!;
       public int TotalQuizzes { get; set; }
       public int  PassedQuizzes { get; set; }
       public double  AverageScore { get; set; }
       public double  TotalScore { get; set;} = 0;
       
    }
}
