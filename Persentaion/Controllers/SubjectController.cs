using AbstractionServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.SubjectDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class SubjectController(ISubjectServices services) : ControllerBase
    {
        [HttpPost("{gradeId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddSubjcet([FromForm] CreateSubjectDto subjectDto, [FromRoute] int gradeId)
        {
            if (ModelState.IsValid)
            {
                var res = await services.AddSubjectAsync(subjectDto, gradeId);
                return Ok(res);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{gradeId}/{subjectCode}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSubject([FromRoute] string subjectCode, [FromRoute] int gradeId)
        {

            var res = await services.DeleteSubjectAsync(subjectCode, gradeId);
            return Ok(res);
        }

        [HttpPatch("{GradeId}/{subjectCode}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSubject([FromBody] UpdateSubjectDto updateSubjectDto, [FromRoute] string subjectCode, [FromRoute] int GradeId)
        {
            if (ModelState.IsValid)
            {
                var res = await services.UPdateSubjectAsync(updateSubjectDto, subjectCode,GradeId);
                return Ok(res);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetAllSubject([FromQuery] Subject_LessonFilteration subjectFilteration)
        {
            if (ModelState.IsValid == false) return BadRequest(ModelState);
            var res = await services.GetAllSubjectAsync(subjectFilteration);
            return Ok(res);
        }
        [HttpGet("Grade/{subjectCode}")]
        [Authorize(Roles ="Admin,Teacher")]
        public async Task<IActionResult> GetSubjectByCode(string subjectCode)
        {
            var res = await services.GetSubjectByCodeAsync(subjectCode);
            return Ok(res);
        }
        [HttpGet("Grade/{gradeId:int}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetAllSubjectByGradeId([FromQuery] Subject_LessonFilteration filter, [FromRoute] int gradeId)
        {
            if (ModelState.IsValid == false) return BadRequest(ModelState);
            var res = await services.GetAllSubjectByGradeIdAsync(filter, gradeId);
            return Ok(res);

        }
    }
}
