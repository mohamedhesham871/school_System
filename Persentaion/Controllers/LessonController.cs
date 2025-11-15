using AbstractionServices;
using Domain.Models;
using Domain.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Lesson_Dto;
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
    public class LessonController(ILessonServices Services):ControllerBase
    {
        //Create New Lesson
        [HttpPost("{subjectCode}")]
        [Authorize(Roles ="Teacher")]
        public async Task<IActionResult> AddLesson([FromRoute] string subjectCode, [FromForm] CreateLessonDto createLessonDto)
        {
            if(ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await Services.AddLesson(subjectCode, createLessonDto,email);
            return Ok(result);
        }
        
        
        //UpladFile
        [HttpPost("UpladFile/{lessonCode}")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> UploadLessonFile([FromRoute]string lessonCode, [FromForm]UploadFileDto uploadFile)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var res = await Services.UploadFile(lessonCode, uploadFile, userId);
                return Ok(res);
            }
            else
                return BadRequest("Invalid Data");
        }
        
        
        //Delete Lesson using Id Of lesson
        [HttpDelete("{lessonCode}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteLesson([FromRoute]string lessonCode)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await Services.DeleteLesson(lessonCode, email);
            return Ok(result);
        }


        //Delete File of Lesson
        [HttpDelete("DeleteFile/{lessonCode}")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> DeleteFile([FromRoute]string lessonCode)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await Services.DeleteFile(lessonCode, userId!);
            return Ok(result);
        }

        //Update Lesson Data
        [HttpPut("{lessonCode}")]
        [Authorize(Roles ="Teacher")]
        public async Task<IActionResult> UpdateLesson([FromRoute] string lessonCode, [FromForm] UpdateLessonDto updateLessonDto)
        { 
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await Services.UpdateLesson(lessonCode, updateLessonDto,userId!);
            return Ok(result);
        }

        //GEt Lesson By Code 
        [HttpGet("{lessonCode}")]
        public async Task<IActionResult> GetLessonByCode([FromRoute]string lessonCode)
        {

            var result = await Services.GetLessonByCode(lessonCode);
            return Ok(result);
        }
        //For Every One
        [HttpGet("{subjectCode}/GetLessonsInSubject")]
        [Authorize]
        public async Task<IActionResult> GetAllLessonOfSubject([FromRoute]string subjectcode,[FromQuery]Subject_LessonFilteration filter)
        {

            var email = User.FindFirstValue(ClaimTypes.Email);
             var role = User.FindFirstValue(ClaimTypes.Role);
            var result = await Services.GetAllLessonsInSubject(subjectcode,filter,email,role);
                return Ok(result);
            
                
        }
   
     
    }
}
