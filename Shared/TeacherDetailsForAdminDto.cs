using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
   public  class TeacherDetailsForAdminDto
    {
        public string? UserId { get; set; }
        public string? FirstName { get; set; } = null!;
        public string? LastName { get; set; } = null!;
        public string? UserName { get; set; } = null!;
        public string? Email { get; set; } = null!;
        public string? PhoneNumber { get; set; } = null!;
        public DateTime HiringDate { get; set; }
        public string Specialization { get; set; } = null!;
        public string Status { get; set; } = null!;
        public ICollection<string>?SubjectsName { get; set; } =[];
        public ICollection<string>? ClassesName { get; set; } = [];

    }
}
