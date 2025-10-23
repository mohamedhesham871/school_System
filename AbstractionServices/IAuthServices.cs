using Shared;
using Shared.IdentityDtos;
using Shared.IdentityDtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractionServices
{
    public interface IAuthServices
    {
        // No Register Method for Users Only Admin Can Create Users
        Task<UserResponseDto> Login(LoginUserDto loginUser);
        Task<UserResponseDto> RefreshToken(string token);
        Task<GenericResponseDto> Logout(string RefreshToken);
        Task<GenericResponseDto> ChangePassword(ChangePasswordDto changePassword,string Token);
        Task<GenericResponseDto> ForgetPassword(ForgetPasswordDto forgetPassword);
        Task<GenericResponseDto> ResetPassword(ResetPasswordDto resetPassword);
        Task<GenericResponseDto> VerifyEmail(VerifyEmailDto verifyEmail);

        Task<UserProfileDto> UserProfile(string Email);
    }
}
