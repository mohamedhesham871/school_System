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
    [Authorize(Roles ="Admin")]
    public  class ClassEntityController(IClassServices services):ControllerBase
    {
        //Add New ClassEntity
        [HttpPost("AddClass/{GradeId}")]
        public async Task<IActionResult> AddClass([FromRoute] int GradeId, [FromBody] ClassCreateOrUpdate create)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await services.AddClass(GradeId, create);
            return Ok(result);
        }

        //Update ClassEntity
        [HttpPatch("UpdateClass/{ClassCode}")]
        public async Task<IActionResult> UpdateClass([FromRoute] string ClassCode, [FromBody] ClassCreateOrUpdate update)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await services.UpdateClass(ClassCode, update);
            return Ok(result);
        }
        //Delete ClassEntity
        [HttpDelete("DeleteClass/{ClassCode}")]
        public async Task<IActionResult> DeleteClass([FromRoute] string ClassCode)
        {
            var result = await services.DeleteClass(ClassCode);
            return Ok(result);
        }

        //Class Details
        [HttpGet("ClassDeatils/{classCode}")]
        public async Task<IActionResult> ClassDetials([FromRoute] string classcode)
        {
            var res = await services.classDetialsResponseDto(classcode);
            return Ok(res);
        }

        //Get All Class
        [HttpGet("AllClasses")]
        public async Task<IActionResult> GetAllClasses([FromQuery]ClassFilteration filter)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var res = services.GetAllClasses(filter);
            return Ok(res);
        }



    }
}
