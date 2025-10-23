using AbstractionServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.QuizDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class QuestionController(IQuestionServices services) : ControllerBase
    {
        private readonly IQuestionServices _services = services;

        [HttpPost("CreateQuestion/{QuizCode}")]
        public async Task<IActionResult> CreateQuestion([FromBody] QuestionCreateOrUpdateDto questionCreateDto, [FromRoute] string QuizCode)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await _services.CreateQuestion(questionCreateDto, QuizCode, email!);
            return Ok(result);
        }

        [HttpPut("UpdateQuestion/{QuizCode}/{QuestionCode}")]
        public async Task<IActionResult> UpdateQuestion([FromBody] QuestionCreateOrUpdateDto questionUpdateDto, [FromRoute] string QuestionCode, [FromRoute] string QuizCode)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await _services.UpdateQuestion(questionCode: QuestionCode,
                                                        QuizCode: QuizCode,
                                                        questionUpdateDto: questionUpdateDto,
                                                        email: email!);
            return Ok(result);
        }

        [HttpDelete("DeleteQuestion/{QuizCode}/{QuestionCode}")]
        public async Task<IActionResult> DeleteQuestion([FromRoute] string QuestionCode, [FromRoute] string QuizCode)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await _services.DeleteQuestion(QuestionCode, QuizCode, email!);
            return Ok(result);
        }

        [HttpGet("GetQuestions/{QuizCode}")]
        public async Task<IActionResult> GetQuestions([FromRoute] string QuizCode, [FromQuery] int PageIndex = 1)
        {
            var result = await _services.GetAllQuestions(QuizCode, PageIndex);
            return Ok(result);

        }

        [HttpGet("GetQuestion/{QuestionCode}")]
        public async Task<IActionResult> GetQuestion([FromRoute] string QuestionCode)
        {

            var result = await _services.GetQuestionByCode(QuestionCode);
            return Ok(result);
        }
    }
}
