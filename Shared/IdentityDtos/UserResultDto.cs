using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.IdentityDtos
{
    public  class UserResultDto
    {
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
