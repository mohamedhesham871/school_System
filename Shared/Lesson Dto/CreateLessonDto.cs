using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Lesson_Dto
{
    public  class CreateLessonDto
    {
        [Required,MaxLength(250)]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Range(1, int.MaxValue, ErrorMessage = "Order must be greater than 0")]
        public int Order { get; set; }  //Order of the lesson in the subject
    }
}
