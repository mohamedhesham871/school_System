using Domain.Models.subject_Lesson;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.QuizDto
{
    public class CreateOrUpdateQuizDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

    }
}
