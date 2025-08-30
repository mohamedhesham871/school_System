using AbstractionServices;
using Microsoft.AspNetCore.Mvc;
using Shared.IdentityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public  class AuthController(IAuthServices services):ControllerBase
    {
        [HttpPost("StudentRegister")]
        public async Task<IActionResult> Register([FromBody] RegisterStudentDto registerStudent)
        {
            var res = await services.RegisterStudent(registerStudent);
            return Ok(res);
        }
        [HttpPost("TeacherRegister")]
        public async Task<IActionResult> Register([FromQuery] RegisterTeacherDto registerTeacher)
        {
            var res = await services.RegisterTeacher(registerTeacher);
            return Ok(res);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto login)
        {
            var res = await services.Login(login);
            return Ok(res);
        }

    }
}
