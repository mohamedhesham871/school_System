using AbstractionServices;
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
    [Route("api/[controller]")]
    public class SubjectController(ISubjectServices services) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddSubjcet([FromForm]CreateSubjectDto subjectDto)
        {
            if (ModelState.IsValid)
            {
                var res = await services.AddSubjectAsync(subjectDto);
                return Ok(res);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{subjectCode}")]
        public async Task<IActionResult> DeleteSubject(string subjectCode)
        {
            var res = await services.DeleteSubjectAsync(subjectCode);
            return Ok(res);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSubject([FromQuery] SubjectFilteration subjectFilteration)
        {
            if (ModelState.IsValid == false) return BadRequest(ModelState);
            var res = await services.GetAllSubjectAsync(subjectFilteration);
            return Ok(res);
        }
        [HttpGet("{subjectCode}")]
        public async Task<IActionResult> GetSubjectByCode(string subjectCode)
        {
            var res = await services.GetSubjectByCodeAsync(subjectCode);
            return Ok(res);
        }
    }
}
