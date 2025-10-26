using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.IdentityDtos.Admin
{
    public  class UpdateAdminProfileDto
    {
  
        public string ?UserName { get; set; }
        public string? Address { get; set; }
        public IFormFile? ProfileImage { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
