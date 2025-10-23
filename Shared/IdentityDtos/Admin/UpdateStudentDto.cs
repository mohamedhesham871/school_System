using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.IdentityDtos.Admin
{
    public class UpdateStudentDto
    {
        public string? FirstName { get; set; } = null!;
        public string? LastName { get; set; } = null!;
        public string? UserName { get; set; } = null!;
        [EmailAddress, Required]
        public string? Email { get; set; } = null!;
        public string? PhoneNumber { get; set; } = null!;
        public DateTime? DateOfBirth { get; set; }
        public Gender? gender { get; set; } //F  Female / M  male 
        public string? Address { get; set; } = null!;
        public IFormFile? ProfileImage { get; set; }
        public UserState? Status { get; set; } = UserState.Active;

        public string? ParentName { get; set; } = null!;
        public string? ParentContact { get; set; } = null!;
        public DateTime? AssignToSchool { get; set; }
        public int? GradeID { get; set; }
    }
}
