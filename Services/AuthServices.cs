using AbstractionServices;
using AutoMapper;
using Domain.Exceptions;
using Domain.Models.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared;
using Shared.IdentityDtos;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Services
{
    public class AuthServices(UserManager<AppUsers> _user, IOptions<JwtToken> options, IMapper mapper, IConfiguration _config) : IAuthServices
    {
        //Log In
        public async Task<UserResponseDto> Login(LoginUserDto loginUser)
        {
            // check if user exist
            var user = await _user.FindByEmailAsync(loginUser.Email);
            if (user is null || !await _user.CheckPasswordAsync(user, loginUser.Password))
                throw new BadRequestException("Password Or Email  Not Vlaid Please try Again");
            var userReponse = new UserResponseDto();

            //Genrate Access Token
            var token = await GenrateToken(user);

            //Add Data Of Reponse 
            userReponse.message = "User Login Successfully";
            userReponse.IsAuthenticated = true;
            userReponse.UserName = user.UserName;
            userReponse.Email = user.Email;
            userReponse.AccessToken = new JwtSecurityTokenHandler().WriteToken(token); ;
            userReponse.AccesstokenExpireTime = token.ValidTo;


            //Check Refresh Token if Exist
            if (user.RefresTokens.Any(i => i.IsActive))
            {
                var activeRefreshToken = user.RefresTokens.Where(i => i.IsActive).SingleOrDefault();
                userReponse.RefreshToken = activeRefreshToken.Token;
                userReponse.RefreshtokenExpireTime = activeRefreshToken.ExpiredOn;
            }
            else
            {
                var newRefreshToken = GenrateRefreshTokenAsync();
                user.RefresTokens.Add(newRefreshToken);
                await _user.UpdateAsync(user);
                userReponse.RefreshToken = newRefreshToken.Token;
                userReponse.RefreshtokenExpireTime = newRefreshToken.ExpiredOn;
            }
            return userReponse;
        }

        //Refresh Token 
        public async Task<UserResponseDto> RefreshToken(string token)
        {
            var userResponse = new UserResponseDto();
            // check User By Token 
            var user = _user.Users.SingleOrDefault(u => u.RefresTokens.Any(t => t.Token == token));

            if (user is null)
            {
                userResponse.message = "Invalid Token";
                userResponse.IsAuthenticated = false;
                return userResponse;
            }
            var oldRefreshToken = user.RefresTokens.SingleOrDefault(t => t.Token == token);
            //check if Token is Active if Go out 
            if (!oldRefreshToken.IsActive)
            {
                userResponse.message = "InActive Token";
                userResponse.IsAuthenticated = false;
                return userResponse;
            }
            // revoke Old Token 
            oldRefreshToken.RevokedOn = DateTime.UtcNow;
            //create New Token
            var newRefreshToken = GenrateRefreshTokenAsync();
            user.RefresTokens.Add(newRefreshToken);
            await _user.UpdateAsync(user);

            //Create New AccessToken and Return Data
            var Accesstoken = await GenrateToken(user);
            userResponse.AccessToken = new JwtSecurityTokenHandler().WriteToken(Accesstoken);
            userResponse.message = "Create Refesh Token Successfully";
            userResponse.IsAuthenticated = true;
            userResponse.UserName = user.UserName;
            userResponse.Email = user.Email;
            userResponse.AccesstokenExpireTime = Accesstoken.ValidTo;
            userResponse.RefreshToken = newRefreshToken.Token;
            userResponse.RefreshtokenExpireTime = newRefreshToken.ExpiredOn;

            return userResponse;

        }

        //Log Out
        public async Task<GenericResponseDto> Logout(string RefreshToken)
        {
            //check on user that Have Token 
            var user = _user.Users.SingleOrDefault(t => t.RefresTokens.Any(s => s.Token == RefreshToken));

            if (user is null)
                throw new NotFoundException("Invalid Token Sent");

            var OldRefreshToken = user.RefresTokens.Single(t => t.IsActive);

            if (OldRefreshToken is null) //Not Sure 
                throw new BadRequestException("No Token Valid");

            // Revoke Old Token 
            OldRefreshToken.RevokedOn = DateTime.UtcNow;
            var res = await _user.UpdateAsync(user);

            if (!res.Succeeded)
            {
                var error = res.Errors.Select(e => e.Description);
                throw new ValidationErrorsExecption(error);
            }
            var response = new GenericResponseDto()
            {
                Message = "Log Out Successfully .",
                IsSuccess = true

            };
            return response;
        }

        //Change password

        public async Task<GenericResponseDto> ChangePassword(ChangePasswordDto changePassword, string token)
        {
            // check User  By Token 


            var user = _user.Users.SingleOrDefault(u => u.RefresTokens.Any(t => t.Token == token));

            if (user is null)
                throw new NotFoundException("Invalid Token Of User.");

            //check if Token Is valid
            var checkvalidToken = user.RefresTokens.SingleOrDefault(r => r.Token == token);
            if (!checkvalidToken.IsActive)
                throw new BadRequestException("Token Is Expired");


            //Update Password   If Old Password Wrong Method Will Handl that 
            var res = await _user.ChangePasswordAsync(user, changePassword.OldPassword, changePassword.NewPassword);

            if (!res.Succeeded)
            {
                var error = res.Errors.Select(d => d.Description);
                throw new ValidationErrorsExecption(error);
            }

            return new GenericResponseDto() { Message = "Password Changed Successfully.", IsSuccess = true };

        }

        public async Task<GenericResponseDto> ForgetPassword(ForgetPasswordDto forgetPassword)
        {

            var user = await _user.FindByEmailAsync(forgetPassword.Email);

            if (user is not null)
            {
                // Send Email To User
                var subject = "Password Reset";
                var token = await _user.GeneratePasswordResetTokenAsync(user);

                var resetUrl = $"{_config["BaseUrl"]}/api/Atuh/ResetPassword?token={WebUtility.UrlEncode(token)}&email={WebUtility.UrlEncode(forgetPassword.Email)}";

                var emailSent = await SendEmail(forgetPassword.Email, resetUrl, subject);
                if (!emailSent)
                    throw new BadRequestException("Failed to send Reset Password  email.");

            }
            // will Not Check if User Exist To Avoid Hackers
            // Send Response
            return new GenericResponseDto() { Message = "IF Email Exist Check Your Email To Rest Password.", IsSuccess = true };

        }

        public async Task<GenericResponseDto> ResetPassword(ResetPasswordDto resetPassword)
        {

            var user = await _user.FindByEmailAsync(resetPassword.Email);

            if (user is null) throw new NotFoundException("Invalid User Email");

            var res = await _user.ResetPasswordAsync(user, resetPassword.Token, resetPassword.NewPassword);
            if (!res.Succeeded)
            {
                var errors = res.Errors.Select(e => e.Description);
                throw new ValidationErrorsExecption(errors);
            }

            return new GenericResponseDto { IsSuccess = true, Message = "Reset Password" };
        }

        public async Task<GenericResponseDto> VerifyEmail(VerifyEmailDto verifyEmail)
        {
                // Find user by email
                var user = await _user.FindByEmailAsync(verifyEmail.Email);
               
                 if (user is null)
                    throw new NotFoundException("Invalid user email.");

                // Try to confirm the email using token
                var result = await _user.ConfirmEmailAsync(user, verifyEmail.Token);
                
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description);
                    throw new ValidationErrorsExecption(errors);
                }

                return new GenericResponseDto
                {
                    IsSuccess = true,
                    Message = "Email verified successfully."
                };
        }

        public async Task<UserProfileDto> UserProfile(string Email)
        {
            var user = await _user.FindByEmailAsync(Email);
            if (user is null) throw new NotFoundException($"user with Email {Email} Not Found Please Enter Valid email");
            var userProfile = mapper.Map<UserProfileDto>(user);
            return userProfile;
        }




        private async Task<JwtSecurityToken> GenrateToken(AppUsers user)
        {
            var jwtToken = options.Value;
            var userClaim = new List<Claim>()
            {
                new Claim (ClaimTypes.Name,user.UserName),
                new Claim (ClaimTypes.Email,user.Email),
            };
            var role = await _user.GetRolesAsync(user);
            foreach (var item in role)
            {
                userClaim.Add(new Claim(ClaimTypes.Role, item));
            }
            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtToken.SecretKey));
            var token = new JwtSecurityToken(
                 issuer: jwtToken.Issuer
                 , audience: jwtToken.Audience
                 , claims: userClaim
                 , expires: DateTime.UtcNow.AddDays(jwtToken.DurationDays)// Token will expire after 7 days
                 , signingCredentials: new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256Signature)
                 );
            return token;
        }

        private RefresTokens GenrateRefreshTokenAsync()
        {
            var ran = new byte[32];
            using var gen = RandomNumberGenerator.Create();
            gen.GetBytes(ran);

            return new RefresTokens
            {
                Token = Convert.ToBase64String(ran),
                ExpiredOn = DateTime.UtcNow.AddDays(30),
                CreatedOn = DateTime.UtcNow
            };
        }

        private string GenrarteOTP()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private async Task<bool> SendEmail(string email, string body, string subject)
        {

            // Send Email Logic Here
            var emailSettings = new MailMessage();

            emailSettings.From = new MailAddress(_config["EmailSettings:From"]);
            emailSettings.To.Add(email);
            emailSettings.Subject = subject;
            emailSettings.Body = $"We received a request to reset your password. Click the link below to set a new one:\n" +
                $" {body}\n This link will expire in 1 hour \n" +
                $" If you did not request a password reset, please ignore this email.";

            using var smtp = new SmtpClient(_config["EmailSettings:SmtpHost"], int.Parse(_config["EmailSettings:SmtpPort"]));
            smtp.EnableSsl = bool.Parse(_config["EmailSettings:EnableSsl"]);
            smtp.Credentials = new NetworkCredential("mmmelkady23@gmail.com", _config["EmailSettings:Password"]);

            try
            {
                await smtp.SendMailAsync(emailSettings);
                return true;
            }
            catch (Exception ex)
            {
                throw new BadRequestException($"Failed to send  email. {ex}");
            }
            finally
            {
                emailSettings.Dispose(); // Dispose of the email message to free resources
            }

        }
    }


}

/*
 ## 1. Authentication & Authorization
- `POST /Api/Auth/VerifyEmail` - Verify email address
- `POST /Api/Auth/ResendVerification` - Resend verification email

## 2. User Management APIs
### Admin
- `GET /Api/Admin/Dashboard` - Get admin dashboard statistics
- `GET /Api/Admin/Profile` - Get admin profile
- `PUT /Api/Admin/Profile` - Update admin profile
- `GET /Api/Admin/Users` - Get all users (paginated)
- `GET /Api/Admin/Users/{id}` - Get user by ID
- `PUT /Api/Admin/Users/{id}/Status` - Activate/Deactivate user
- `DELETE /Api/Admin/Users/{id}` - Delete user
- `GET /Api/Admin/Analytics` - Get system analytics
- `GET /Api/Admin/Reports` - Generate system reports

### Teacher
- `GET /Api/Teacher/Dashboard` - Get teacher dashboard
- `GET /Api/Teacher/Q` - Get teacher profile
- `PUT /Api/Teacher/Profile` - Update teacher profile
- `GET /Api/Teacher/MyClasses` - Get assigned classes
- `GET /Api/Teacher/MySubjects` - Get assigned subjects
- `GET /Api/Teacher/Schedule` - Get teaching schedule
- `GET /Api/Teacher/Students` - Get all students in teacher's classes

### Student
- `GET /Api/Student/Dashboard` - Get student dashboard
- `GET /Api/Student/Profile` - Get student profile
- `PUT /Api/Student/Profile` - Update student profile
- `GET /Api/Student/MyClasses` - Get enrolled classes
- `GET /Api/Student/MySubjects` - Get enrolled subjects
- `GET /Api/Student/Schedule` - Get class schedule
- `GET /Api/Student/Grades` - Get all grades/results*/


    //public async Task<UserResultDto> RegisterStudent(RegisterStudentDto registerUser)
    //{
    //    // Check if user already exist
    //    var user = await  _user.FindByEmailAsync(registerUser.Email);
    //    if (user is not null) throw new BadRequestException($"user with Email {registerUser.Email} Already Exist Please Enter Another email");

            //    //Check if Password is match with Confirm Password
            //    if (registerUser.Password != registerUser.confirmPassword) throw new BadRequestException("Password and Confirm Password do not match");

            //    //chechk if profile image not null
            //    var UserPictureProfile = "images/Default_Icone.png";

            //    if (registerUser.ProfileImage != null && registerUser.ProfileImage.Length > 0)
            //    {
            //        var UploadImage = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            //        var fileName = $"{Guid.NewGuid()}_{registerUser.ProfileImage.FileName}";
            //        var filePath = Path.Combine(UploadImage, fileName);

            //        using (var Stream = new FileStream(filePath, FileMode.Create))
            //        {
            //            await registerUser.ProfileImage.CopyToAsync(Stream);
            //        }
            //        UserPictureProfile = $"/images/{fileName}"; // Set the new profile picture path
            //    }

            //    // Add New User
            //    var Student = mapper.Map<Students>(registerUser);
            //    Student.ProfileImage = UserPictureProfile;

            //    var result = await _user.CreateAsync(Student, registerUser.Password);
            //    if (!result.Succeeded)
            //    {
            //        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            //        throw new BadRequestException($"Failed to create user: {errors}");
            //    }

            //    // Assign Role to User
            //    await _user.AddToRoleAsync(Student, "Student");
            //    return new UserResultDto
            //    {
            //        UserEmail = Student.Email!,
            //        UserName = Student.UserName!,
            //        Token = await GenrateToken(Student)
            //    };

            //}