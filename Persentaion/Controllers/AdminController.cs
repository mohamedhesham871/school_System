using AbstractionServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.IdentityDtos;
using Shared.IdentityDtos.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    //[Authorize(Roles = "Admin")]
    public class AdminController(IAdminServices services) : ControllerBase
    {
        //Add New Teacher 
        [HttpPost("CreateTeacher")]
        public async Task<IActionResult> CreateTeacher([FromForm] CreateTeacherDto teacherDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdTeacher = await services.AddTeacher(teacherDto);
            return Ok(createdTeacher);
        }
        //Add New Student
        [HttpPost("CreateStudent")]
        public async Task<IActionResult> CreateStudent([FromForm] CreateStudentDto studentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdStudent = await services.AddStudent(studentDto);
            return Ok(createdStudent);
        }

        //Delete User By Id
        [HttpDelete("DeleteUser/{UserId}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string UserId)
        {
            var result = await services.DeleteUser(UserId);
            return Ok(result);
        }
        //UPdate Student info By Id
        [HttpPut("UpdateStudent/{UserId}")]
        public async Task<IActionResult> UpdateStudent([FromRoute] string UserId, [FromForm] UpdateStudentDto updateStudentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedStudent = await services.UpdateStudent(updateStudentDto, UserId);
            return Ok(updatedStudent);
        }

        //Update Teacher info By Id
        [HttpPut("UpdateTeacher/{UserId}")]
        public async Task<IActionResult> UpdateTeacher([FromRoute] string UserId, [FromForm] UpdateTeacherDto updateTeacherDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedTeacher = await services.UpdateTeacher(updateTeacherDto, UserId);
            return Ok(updatedTeacher);
        }

        //Assign Student to Class
        [HttpPost("AssignStudentToClass/{StudentId}/{ClassCode}")]
        public async Task<IActionResult> AssignStudentToClass([FromRoute] string StudentId, [FromRoute] string ClassCode)
        {
            var result = await services.AssignStudentToClass(StudentId, ClassCode);
            return Ok(result);
        }

        //Assign Student to Subject
        [HttpPost("AssignStudentToSubject/{StudentId}/{SubjectCode}")]
        public async Task<IActionResult> AssignStudentToSubject([FromRoute] string StudentId, [FromRoute] string SubjectCode)
        {
            var result = await services.AssingStudentToSubject(StudentId, SubjectCode);
            return Ok(result);
        }

        //Assign Teacher to Class
        [HttpPost("AssignTeacherToClass/{TeacherId}/{ClassCode}")]
        public async Task<IActionResult> AssignTeacherToClass([FromRoute] string TeacherId, [FromRoute] string ClassCode)
        {
            var result = await services.AssignTeacherToClass(TeacherId, ClassCode);
            return Ok(result);

        }

        //Assign Teacher to Subject
        [HttpPost("AssignTeacherToSubject/{TeacherId}/{SubjectCode}")]
        public async Task<IActionResult> AssignTeacherToSubject([FromRoute] string TeacherId, [FromRoute] string SubjectCode)
        {
            var result = await services.AssingTeacherToSubject(TeacherId, SubjectCode);
            return Ok(result);
        }

        //Remove Student from Class
        [HttpDelete("RemoveStudentFromClass/{StudentId}/{ClassCode}")]
        public async Task<IActionResult> RemoveStudentFromClass([FromRoute] string StudentId, [FromRoute] string ClassCode)
        {
            var result = await services.RemoveStudentFromClass(StudentId, ClassCode);
            return Ok(result);
        }
        //Remove Student from Subject
        [HttpDelete("RemoveStudentFromSubject/{StudentId}/{SubjectCode}")]
        public async Task<IActionResult> RemoveStudentFromSubject([FromRoute] string StudentId, [FromRoute] string SubjectCode)
        {
            var result = await services.RemoveStudentFromSubject(StudentId, SubjectCode);
            return Ok(result);

        }

        //Remove Teacher from Class
        [HttpDelete("RemoveTeacherFromClass/{TeacherId}/{ClassCode}")]
        public async Task<IActionResult> RemoveTeacherFromClass([FromRoute] string TeacherId, [FromRoute] string ClassCode)
        {
            var result = await services.RemoveTeacherFromClass(TeacherId, ClassCode);
            return Ok(result);
        }

        //Remove Teacher from Subject
        [HttpDelete("RemoveTeacherFromSubject/{TeacherId}/{SubjectCode}")]
        public async Task<IActionResult> RemoveTeacherFromSubject([FromRoute] string TeacherId, [FromRoute] string SubjectCode)
        {
            var result = await services.RemoveTeacherFromSubject(TeacherId, SubjectCode);
            return Ok(result);
        }
         //Get Admin Profile
        [HttpGet("AdminProfile/{UserId}")]
        public async Task<IActionResult> ProfileAdmin([FromRoute] string UserId)
        {
            var res = await services.getAdminProfile(UserId);
            return Ok(res);
        }
        //Update Admin profile
        [HttpPut("AdminProfile/{UserId}")]
        public async Task<IActionResult> AdminProfile([FromRoute] string UesrId, [FromForm] UpdateAdminProfileDto Update, string UserId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var res = await services.updateAdminProfile(UserId, Update);
            return Ok(res);
        }

        [HttpGet("Dashboard")]
        public async Task<IActionResult> DashBoard()
        {
            var res= await services.adminDashboardDto();
            return Ok(res);
        }

        [HttpGet("GetAllStudets")]
        public async Task<IActionResult>GetAllStudents([FromQuery]USerFilteration f)
        {
            var res = await services.GetAllStudents(f);
            return Ok(res);

        }

        [HttpGet("GetStudent/{studentId}")]
        public async Task<IActionResult> GetStudent([FromRoute] string studentId)
        {
            var res = await services.GetStudentDetailsByCode(studentId);
            return Ok(res);
        }

        [HttpGet("GetTeachers")]
        public async Task<IActionResult> GetAllTeachers([FromQuery] USerFilteration f)
        {
            var res = await services.GetAllTeachers(f);
            return Ok(res);
        }
        [HttpGet("GetTeacher/{teacherId}")]
        public async Task<IActionResult> GetTeacher([FromRoute] string teacherId)
        {
            var res = await services.GetStudentDetailsByCode(teacherId);
            return Ok(res);
        }
    }
}
