using Domain.Models.User;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Shared.SubjectDtos
{
        public class CreateSubjectDto
        {
            [Required(ErrorMessage = "Subject code is required")]
            [MaxLength(20, ErrorMessage = "Subject code cannot exceed 20 characters")]
            public string SubjectCode { get; set; } = string.Empty;

            [Required(ErrorMessage = "Subject name is required")]
            [MaxLength(100, ErrorMessage = "Subject name cannot exceed 100 characters")]
            public string SubjectName { get; set; } = string.Empty;

            [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
            public string Description { get; set; } = string.Empty;

            public string? TeacherId { get; set; }

            [Required(ErrorMessage = "Grade is required")]
            public int GradeId { get; set; }
        }
    
}
