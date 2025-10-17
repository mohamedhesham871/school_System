using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.IdentityDtos
{
    public class UserResponseDto
    {
        public string message { get; set; }
        public bool IsAuthenticated { get; set; }

        public string? UserName { get; set; }
        public string? Email { get; set; }
        //public List<string>? Roles { get; set; }
        public string? AccessToken { get; set; }
        public DateTime? AccesstokenExpireTime { get; set; }
        [JsonIgnore]       //Refresh Token not show in response can Send it at Cookies
        public string RefreshToken { get; set; }
        public DateTime RefreshtokenExpireTime { get; set; }

    }
        
}
