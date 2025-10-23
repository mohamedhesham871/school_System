using Domain.Models.User;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.IdentityDtos
{
    public  class UpdateTeacherDto
    {
        public string? FirstName { get; set; } = null!;
        public string? LastName { get; set; } = null!;
        public string? Email { get; set; } = null!;
        public string? UserName { get; set; } = null!;
        public string? PhoneNumber { get; set; } = null!;
        public string? Address { get; set; } = null!;
        public IFormFile? ProfileImage { get; set; }
        public string? Specialization { get; set; } = null!;
        public UserState? Status { get; set; }
        public DateTime ?HiringDate { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Gender? Gender { get; set; } = null!;

    }
}
