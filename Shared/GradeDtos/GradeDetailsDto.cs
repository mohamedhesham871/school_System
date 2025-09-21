using Shared.SubjectDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.GradeDtos
{
    public  class GradeDetailsDto
    {
        public string Description { get; set; } = string.Empty;
        public string GradeCode { get; set; } = string.Empty;
        public string GradeName { get; set; } = string.Empty;
        public string AcademicYear { get; set; } = string.Empty;
        public List<SubjectResponseShortDto?> Subjects { get; set; } = new();

    }
}
