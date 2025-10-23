//using AbstractionServices;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Shared;
//using Shared.IdentityDtos;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Presentation.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//  //  [Authorize(Roles = "Admin")]
//    public class TeacherController(ITeacherService service):ControllerBase
//    {
//        [HttpGet("GetAllTeachers")]
//        public async Task<ActionResult<PaginationResponse<TeacherResultDto>>> GetAll([FromQuery] TeacherFilteration teacherFilteration)
//        {
//            var res = await  service.GetAllTeachersAsync(teacherFilteration);
//            return Ok(res);
//        }
//        [HttpGet("{teacherId}")]
//        public async Task<ActionResult<TeacherResultDto>> GetByIdAsync([FromRoute] string teacherId)
//        {
//            var res = await service.GetTeacherByIdAsync(teacherId);
//            return Ok(res);
//        }
//        //Get Teacher By 
//        [HttpPost("Email")]
//        public async Task<ActionResult<TeacherResultDto>> GetByEmail([FromForm]string Email)
//        {
//            var res = await service.FindTeacherByEmailAsync(Email);
//            return Ok(res);
//        }
//        [HttpPost]//Add new Teacher
//        public async  Task<IActionResult> AddTeacher([FromForm] NewTeacherDto newTeacher)
//        {
//             var res =await service.AddTeacherAsync(newTeacher);
//            return Ok(res);
//        }
//        [HttpPut("{teacherId}")]
//        public async Task<IActionResult> UpdateTeacher([FromForm] UpdateTeacherDto updateTeacher, [FromRoute] string teacherId)
//        {
//           await  service.UpdateTeacherAsync( teacherId, updateTeacher);

//            return Ok("Update Teacher successfully ");
//        }
//        [HttpDelete("{teacherId}")]
//        public async Task<IActionResult> DeleteTeacher([FromRoute] string teacherId)
//        {
//          var res=await   service.DeleteTeacherAsync(teacherId);
//            return Ok(res);
//        }

//    }
//}
