using AbstractionServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared.IdentityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthServices services) : ControllerBase
    {
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var res = await services.Login(login);
          
            if (!res.IsAuthenticated)
                return BadRequest(res.message);

            if (!string.IsNullOrEmpty(res.RefreshToken))
                SetRefshTokenInCookies(res.RefreshToken, res.RefreshtokenExpireTime);
            return Ok(res);
        }
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var token = Request.Cookies["RefreshToken"];
            if (string.IsNullOrEmpty(token))
                return BadRequest("Invalid Token");
            var res = await services.RefreshToken(token);

            if (!res.IsAuthenticated)
                return BadRequest(res.message);

            SetRefshTokenInCookies(res.RefreshToken, res.RefreshtokenExpireTime);

            return Ok(res);
        }
        [HttpGet("Profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            
            var EmailClaim = User.FindFirstValue(ClaimTypes.Email);
            var res = await services.UserProfile(EmailClaim!);
            return Ok(res);
        }

        private void  SetRefshTokenInCookies(string refreshToken ,DateTime ExpireRefreshToken)
        {
            var cookies = new CookieOptions()
            {
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                IsEssential = true,
                Expires = ExpireRefreshToken.ToLocalTime()
            };
            Response.Cookies.Append("RefreshToken", refreshToken, cookies);
        }
        
    }
}
