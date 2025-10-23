using AbstractionServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.QuizDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AnswerController(IAnswerServices services) : ControllerBase
    {
        [HttpGet("{AnswerCode}")]
        public async Task<IActionResult> GetAnswerById([FromRoute]string AnswerCode)
        {

            var answer = await services.GetAnswerByCode(AnswerCode );
            return Ok(answer);
        }

        [HttpPost("createAnswer/{questionCode}")]
        public async Task<IActionResult> CreateAnswer([FromBody] AnswerCreateOrUpdateDto answerDto, [FromRoute]string questionCode)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdAnswer = await services.createAnswer(answerDto,questionCode);

            return Ok(createdAnswer);
        }
        [HttpPut("{AnswerCode}")]
        public async Task<IActionResult> UpdateAnswer( [FromBody] AnswerCreateOrUpdateDto answerDto, [FromRoute]string AnswerCode)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var updatedAnswer = await services.UpdateAnswer(answerDto,AnswerCode);
          
            return Ok(updatedAnswer);
        }
        [HttpDelete("{AnswerCode}")]
        public async Task<IActionResult> DeleteAnswers([FromRoute]string AnswerCode)
        {
            var result = await services.DeleteAnswer(AnswerCode);
            return Ok(result);
        }

        [HttpGet("AllAnswersForQuestion/{questionCode}")]
        public async Task<IActionResult> GetAllAnswersForQuestion([FromRoute]string questionCode)
        {
            var answers = await services.GetAllAnswersOfQuestion(questionCode);
            return Ok(answers);
        }
    }
}
