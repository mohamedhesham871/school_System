using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.IdentityDtos
{
    public  class TeacherResultDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string gender { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string ProfileImage { get; set; } = null!;
        public DateTime HiringDate { get; set; }
        public string Specialization { get; set; } = null!;
        public string Status { get; set; } = null!;
        public IList<int>? AssignedSubjects { get; set; } = new List<int>(); 
        public IList<int>? AssignedClasses { get; set; } = new List<int>(); 

    }
}
