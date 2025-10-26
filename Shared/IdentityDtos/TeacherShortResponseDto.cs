using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.IdentityDtos
{
    public class TeacherShortResponseDto
    {
        public string? userId { get;set; }
        public string? FirstName { get; set; } = null!;
        public string ?UserName { get; set; } = null!;
        public string ?Email { get; set; } = null!;
        public string? phoneNumber { get; set; } = null!;
        public string ?Specialization { get; set; } = null!;
        public ICollection<string>? className { get; set; }
        public ICollection<string>? subjectAssignName {  get; set; }
        public string ?status { get; set; }

    }
}
