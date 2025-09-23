using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Lesson_Dto
{
    public class UpdateLessonDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? MaterialUrl { get; set; }
        public int? Order { get; set; }  //Order of the lesson in the subject
        public bool? IsActive { get; set; }
    }
}
