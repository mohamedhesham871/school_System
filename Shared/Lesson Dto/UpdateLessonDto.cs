using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Lesson_Dto
{
    public class UpdateLessonDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Order must be greater than 0")]
        public int? Order { get; set; }  //Order of the lesson in the subject
        public bool? IsActive { get; set; }
    }
}
