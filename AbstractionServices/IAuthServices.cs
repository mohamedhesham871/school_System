using Shared.IdentityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractionServices
{
    public interface IAuthServices
    {
        Task<UserResultDto> Login(LoginUserDto loginUser);
        Task<UserResultDto> RegisterStudent(RegisterStudentDto registerUser);
        Task<UserResultDto> RegisterTeacher(RegisterTeacherDto registerUser);
    }
}
