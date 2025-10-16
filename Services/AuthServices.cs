using AbstractionServices;
using AutoMapper;
using Domain.Exceptions;
using Domain.Models.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared;
using Shared.IdentityDtos;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthServices(UserManager<AppUsers> _user,IOptions<JwtToken> options,IMapper mapper) : IAuthServices
    {
        public async Task<UserResultDto> Login(LoginUserDto loginUser)
        {
            // check if user exist
            var user = await  _user.FindByEmailAsync(loginUser.Email);
            if (user is null) throw new NotFoundException($"user with Email {loginUser.Email} Not Found Please Enter Valid email");
            // check if password is correct
            var isCorrect = await _user.CheckPasswordAsync(user, loginUser.Password);
            if (!isCorrect) throw new BadRequestException("Password is Incorrect Please Enter Valid Password");
           
            return new UserResultDto
            {
                UserEmail = user.Email!,
                UserName = user.UserName!,
                Token = await GenrateToken(user)
            };
        }

        public async Task<UserResultDto> RegisterStudent(RegisterStudentDto registerUser)
        {
            // Check if user already exist
            var user = await  _user.FindByEmailAsync(registerUser.Email);
            if (user is not null) throw new BadRequestException($"user with Email {registerUser.Email} Already Exist Please Enter Another email");

            //Check if Password is match with Confirm Password
            if (registerUser.Password != registerUser.confirmPassword) throw new BadRequestException("Password and Confirm Password do not match");

            //chechk if profile image not null
            var UserPictureProfile = "images/Default_Icone.png";

            if (registerUser.ProfileImage != null && registerUser.ProfileImage.Length > 0)
            {
                var UploadImage = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                var fileName = $"{Guid.NewGuid()}_{registerUser.ProfileImage.FileName}";
                var filePath = Path.Combine(UploadImage, fileName);

                using (var Stream = new FileStream(filePath, FileMode.Create))
                {
                    await registerUser.ProfileImage.CopyToAsync(Stream);
                }
                UserPictureProfile = $"/images/{fileName}"; // Set the new profile picture path
            }

            // Add New User
            var Student = mapper.Map<Students>(registerUser);
            Student.ProfileImage = UserPictureProfile;
            
            var result = await _user.CreateAsync(Student, registerUser.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BadRequestException($"Failed to create user: {errors}");
            }

            // Assign Role to User
            await _user.AddToRoleAsync(Student, "Student");
            return new UserResultDto
            {
                UserEmail = Student.Email!,
                UserName = Student.UserName!,
                Token = await GenrateToken(Student)
            };

        }

       
    
        public async Task<UserProfileDto> UserProfile(string Email)
        {
            var user =await  _user.FindByEmailAsync(Email);
            if (user is null) throw new NotFoundException($"user with Email {Email} Not Found Please Enter Valid email");
            var userProfile = mapper.Map<UserProfileDto>(user);
            return userProfile;
        }


        public async Task<string> GenrateToken(AppUsers user)
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
            // Generate the token string
            var tokenHandler = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenHandler;

         
        }

        
    }
   
}
/*
 ## 1. Authentication & Authorization
- `POST /Api/Auth/Login` - User login
- `POST /Api/Auth/Logout` - User logout
- `POST /Api/Auth/RefreshToken` - Refresh access token
- `POST /Api/Auth/ChangePassword` - Change current password
- `POST /Api/Auth/ForgotPassword` - Request password reset
- `POST /Api/Auth/ResetPassword` - Reset password with token
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