using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.IdentityDtos
{
    public class UserProfileDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
       
        public DateOnly DateOfBirth { get; set; }
        public string gender { get; set; } 
        public string Address { get; set; } = null!;
        public string ProfileImage { get; set; }
        public string Status { get; set; }
    }
}
