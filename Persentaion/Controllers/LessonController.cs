using AbstractionServices;
using Microsoft.AspNetCore.Mvc;
using Shared.Lesson_Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonController(ILessonServices Services):ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddLesson([FromQuery] string subjectCode, [FromForm] CreateLessonDto createLessonDto)
        {
            if(ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await Services.AddLesson(subjectCode, createLessonDto);
            return Ok(result);
        }
    }
}
