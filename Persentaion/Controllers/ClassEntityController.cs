using AbstractionServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public  class ClassEntityController(IClassServices services):ControllerBase
    {
        //Add New ClassEntity
        [HttpPost("AddClass/{GradeId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddClass([FromRoute] int GradeId, [FromBody] ClassCreateOrUpdate create)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await services.AddClass(GradeId, create);
            return Ok(result);
        }

        //Update ClassEntity
        [HttpPatch("UpdateClass/{ClassCode}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateClass([FromRoute] string ClassCode, [FromBody] ClassCreateOrUpdate update)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await services.UpdateClass(ClassCode, update);
            return Ok(result);
        }
        //Delete ClassEntity
        [HttpDelete("DeleteClass/{ClassCode}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteClass([FromRoute] string ClassCode)
        {
            var result = await services.DeleteClass(ClassCode);
            return Ok(result);
        }

        //Class Details
        [HttpGet("ClassDeatils/{classCode}")]
        [Authorize(Roles ="Admin,Teacher,Student")]
        public async Task<IActionResult> ClassDetials([FromRoute] string classCode)
        {
            var res = await services.classDetialsResponseDto(classCode);
            return Ok(res);
        }

        //Get All Class
        [HttpGet("AllClasses")]
        [Authorize(Roles ="Admin,Teacher")]
        public async Task<IActionResult> GetAllClasses([FromQuery]ClassFilteration filter)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var res = await services.GetAllClasses(filter);
            return Ok(res);
        }



    }
}
