using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.QuizDto
{
    public class QuizStudentResultInLesson
    {

        public string StudentName { get; set; } = null!;
       // public string QuizTitle { get; set; } = null!;
        public DateTime TakeTime { get; set; }
        public double Score { get; set; }
        public bool IsPassed { get; set; }

    }
}
