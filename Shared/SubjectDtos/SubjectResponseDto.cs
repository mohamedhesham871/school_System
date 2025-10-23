using Domain.Models;
using Domain.Models.User;
using Shared.GradeDtos;
using Shared.IdentityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.SubjectDtos
{
    public class SubjectResponseDto
    {
        public string SubjectName { get; set; } = null!;
        public string SubjectCode { get; set; } = null!;
        public string Description { get; set; } = string.Empty;

        public TeacherShortResponseDto? Teacher { get; set; } // Navigation property to Teacher
        public GradeResponseShortDto Grade { get; set; } = null!; // Navigation property to Grade
        public List<LessonShortResponseDto?> Lessons { get; set; } = new();

    }
}
