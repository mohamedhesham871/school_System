using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.IdentityDtos
{
    public class RegisterStudentDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public Gender gender { get; set; }  
        public string Address { get; set; } = null!;
        public IFormFile? ProfileImage { get; set; }
        [EmailAddress, Required]
        public string Email { get; set; } = null!;
        [Required, MinLength(6)]
        public string Password { get; set; } = null!;
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match.")]
        public string confirmPassword { get; set; } = null!;
        public string? ParentName { get; set; } = null!;
        public string? ParentContact { get; set; } = null!;
        public DateTime EnrollmentDate { get; set; }
        public int? GradeID { get; set; } 
        public int? ClassID { get; set; } 
        public StudentState Status { get; set; } = StudentState.Active;
    }
}
