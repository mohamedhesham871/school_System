using AbstractionServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared.IdentityDtos.Auth;
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

        [HttpPost("LogOut")]
        public async Task<IActionResult> LogOut()
        {
            var token = Request.Cookies["RefreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest("Invalid Token");

            var res = await services.Logout(token);
            return Ok(res);

        }
       
        [HttpPost("ChangePasswrod")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromForm]ChangePasswordDto changePassword)
        {
            var token = Request.Cookies["RefreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest("Invalid Token");

            var res = await services.ChangePassword(changePassword, token);
            return Ok(res);

        }
        
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordDto forgetPassword)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var res = await services.ForgetPassword(forgetPassword);
            return Ok(res);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPasswrod([FromBody] ResetPasswordDto resetPassword)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var res =await services.ResetPassword(resetPassword);

            return Ok(res);
        }

        [HttpPost("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailDto verifyEmail)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var res = await services.VerifyEmail(verifyEmail);
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
