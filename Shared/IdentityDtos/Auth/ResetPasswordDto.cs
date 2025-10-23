using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.IdentityDtos.Auth
{
    public class ResetPasswordDto
    {

        public string? Email { get; set; }
        public string? Token { get; set; }

        [Required(ErrorMessage = "Password is required")]

        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
        [RegularExpression(
                   @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
              ErrorMessage = "Password must have at least one uppercase letter, one lowercase letter, one number, and one special character"
               )]
        public required string NewPassword { get; set; }
        [Compare("NewPassword", ErrorMessage = "Password Doesn't Match. ")]
        public required string ComfirmPassword { get; set; }

    }
}
