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

    public class QuizController (IQuizServices services): ControllerBase
    {
        private readonly IQuizServices _services = services;
        // 1 - Create Quiz
        [HttpPost("CreateQuiz/{LessonCode}")] 
        public async Task<IActionResult> CreateQuiz([FromBody] CreateOrUpdateQuizDto quizCreateDto,[FromRoute]string LessonCode)
        {
            if(ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _services.CreateQuizAsync(quizCreateDto,LessonCode);
            return Ok(result);
        }

        //2 - Update Quiz
        [HttpPut("UpdateQuiz/{QuizCode}")]
        [Authorize (Roles = "Teacher")]
        public async Task<IActionResult> UpdateQuiz([FromBody] CreateOrUpdateQuizDto quizUpdateDto, [FromRoute] string QuizCode)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var email =   User.FindFirstValue(ClaimTypes.Email);
            var result = await _services.UpdateQuizAsync(QuizCode, quizUpdateDto, email);
            return Ok(result);
        }
        //3 - Delete Quiz
        [HttpDelete("DeleteQuiz/{QuizCode}")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> DeleteQuiz([FromRoute] string QuizCode)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
           var res=  await _services.DeleteQuizAsync(QuizCode, email);
            return Ok(res);
        }

        //4 - Get Quiz By Code With Questions
        [HttpGet("GetQuizByCode/{QuizCode}")]
        public async Task<IActionResult> GetQuizByCode([FromRoute] string QuizCode)
        {
            var result = await _services.GetAllQuestionsInQuizByQuizCodeAsync(QuizCode);
            return Ok(result);
        }
        //5- Get Quiz By Code Questions With Answers
        [HttpGet("GetQuizByCodeWithAnswers/{QuizCode}")]
        public async Task<IActionResult> GetQuizByCodeWithAnswerAsync([FromRoute] string QuizCode)
        {
            var result = await _services.GetQuizByCodeAsync(QuizCode);
            return Ok(result);
        }

        //6 - Get All Quizzes InSubject
        [HttpGet("GetAllQuizzes/{SubjectCode}")]
        public async Task<IActionResult> GetAllQuizzes([FromRoute] string SubjectCode)
        {
            var result = await _services.GetAllQuizesInSubjectBySubjectCodeAsync(SubjectCode);
            return Ok(result);
        }

        //7 - Get All Quizzes Result In Subject
        [HttpGet("GetAllResultQuizOfSubject/{SubjectCode}")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetAllResultQuizOfSubject([FromRoute] string SubjectCode)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await _services.GetAllQuizResultsOfSubjectByCodeAsync(SubjectCode,email);
            return Ok(result);
        }

        //8- Get All Quizzes Result In Lesson   
        [HttpGet("GetAllResultQuizOfLesson/{LessonCode}")]
        public async Task<IActionResult> GetAllResultQuizOfLesson([FromRoute] string LessonCode)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await _services.GetAllQuizStatisticsInLessonByCodeAsync(LessonCode, email);
            return Ok(result);
        }

        //9 - GetAllStudentAttemptsAsync 
        [HttpGet("GetAllStudentAttempts/{QuizCode}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetAllStudentAttemptsAsync([FromRoute] string QuizCode)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var StudentUserName = email; // Assuming the username is the email, adjust as necessary
            var result = await _services.GetAllStudentAttemptsAsync(StudentUserName, QuizCode);
            return Ok(result);
        }

        //10 - SubmitQuizAttemptAsync
        [HttpPost("SubmitQuizAttempt/{QuizCode}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> SubmitQuizAttemptAsync([FromRoute] string QuizCode, [FromBody] List<AnswerSubmissionDto> answers)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var email = User.FindFirstValue(ClaimTypes.Email);
            var StudentUserName = email; // Assuming the username is the email, adjust as necessary
            var result = await _services.SubmitQuizAttemptAsync(QuizCode, StudentUserName, answers);
            return Ok(result);
        }


    }
}
