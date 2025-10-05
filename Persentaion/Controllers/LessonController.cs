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
    public class LessonController(ILessonServices Services):ControllerBase
    {
        //Create New Lesson
        [HttpPost]
        [Authorize(Roles ="Teacher")]
        public async Task<IActionResult> AddLesson([FromQuery] string subjectCode, [FromForm] CreateLessonDto createLessonDto)
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
        [HttpPost("UpladFile")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> UploadLessonFile(string LessonCode, UploadFileDto uploadFile)
        {
            if (ModelState.IsValid)
            {
                var res = await Services.UploadFile(LessonCode, uploadFile);
                return Ok(res);
            }
            else
                return BadRequest("Invalid Data");
        }
        
        
        //Delete Lesson using Id Of lesson
        [HttpDelete("{LessonCode}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteLesson(string lessonCode)
        {
            var result = await Services.DeleteLesson(lessonCode);
            return Ok(result);
        }


        //Delete File of Lesson
        [HttpDelete("DeleteFile/{LessonCode}")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> DeleteFile([FromRoute]string lessonCode)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await Services.DeleteFile(lessonCode, email);
            return Ok(result);
        }

        //Update Lesson Data
        [HttpPut("{lessonCode}")]
        [Authorize(Roles ="Teacher")]
        public async Task<IActionResult> UpdateLesson( string lessonCode, [FromForm] UpdateLessonDto updateLessonDto)
        { 
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var email= User.FindFirstValue(ClaimTypes.Email);
            var result = await Services.UpdateLesson(lessonCode, updateLessonDto,email);
            return Ok(result);
        }

        //GEt Lesson By Code 
        [HttpGet("{lessonCode}")]
        public async Task<IActionResult> GetLessonByCode(string lessonCode)
        {

            var result = await Services.GetLessonByCode(lessonCode);
            return Ok(result);
        }
        //For Every One
        [HttpGet("GetAllLessonsInSubject")]
        [Authorize]
        public async Task<IActionResult> GetAllLessonOfSubject(string subjectcode,[FromQuery]Subject_LessonFilteration filter)
        {

            var email = User.FindFirstValue(ClaimTypes.Email);
             var role = User.FindFirstValue(ClaimTypes.Role);
            var result = await Services.GetAllLessonsInSubject(subjectcode,filter,email,role);
                return Ok(result);
            
                
        }
   
     
    }
}
