using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.IdentityDtos.Admin
{
    public class AdminProfileDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; } 
        public string? Address { get; set; }
        public string? ProfileImage { get; set; } 
        public string? PhoneNumber { get; set; }

    }
}
