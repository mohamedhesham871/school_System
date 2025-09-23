using AbstractionServices;
using Domain.Contract;
using Domain.Exceptions;
using Domain.Models;
using Microsoft.Extensions.Logging;
using Services.SpecificationsFile;
using Shared;
using Shared.Lesson_Dto;
using Shared.SubjectDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Services
{
    public class LessonServices(IUnitOfWork unitOfWork,ILogger<LessonServices>_logger ) : ILessonServices
    {
        public async Task<string> AddLesson(string subjectCode, CreateLessonDto createLessonDto)
        {
            try
            {
                //1- check on Subject exist
                var subject = await unitOfWork.GetRepository<Subject, int>().GetEntityWithCode<Subject>(subjectCode);
                if (subject is null) throw new NotFoundException($"Subject Code is Not Found Please Try Valid");
                #region Upload File 
                //if MatrialUrl is not null check if its valid url

                var LessonFinalFile = string.Empty;
                if (createLessonDto.MaterialUrl is not null)
                { 
                
                //1- check on Extension
                var Extention = Path.GetExtension(createLessonDto.MaterialUrl.FileName).ToLowerInvariant();
                    if(Extention != ".pdf" && Extention != ".docx" && Extention != ".pptx" && Extention != ".mp4")
                    {
                        throw new InvalidOperationException("Invalid File Extension. Only PDF, DOCX, PPTX, and MP4 are allowed.");
                    }
                    //2- check on MIME type
                    var LessonFileType = createLessonDto.MaterialUrl.ContentType;
                    if (LessonFileType != "application/pdf" &&   // PDF
                        LessonFileType != "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && // DOCX
                        LessonFileType != "application/vnd.openxmlformats-officedocument.presentationml.presentation"&& // PPTX
                        LessonFileType != "video/mp4")// MP4
                    {
                        throw new InvalidOperationException("Invalid File Type. Only PDF, DOCX, PPTX, and MP4 are allowed.");
                    }
                    //3- save file to server
                    var UploadFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/FolderFile/Lessons");
                    var fileName = $"{Guid.NewGuid()}_{createLessonDto.MaterialUrl.FileName}";
                    var filePath = Path.Combine(UploadFile, fileName);

                    using (var Stream = new FileStream(filePath, FileMode.Create))
                    {
                        await createLessonDto.MaterialUrl.CopyToAsync(Stream);
                    }
                    LessonFinalFile = $"/FolderFile/Lessons/{fileName}";
                }
                #endregion
                var lesson = new Lesson()
                {
                    Title = createLessonDto.Title,
                    Description = createLessonDto.Description,
                    MaterialUrl = createLessonDto.MaterialUrl is not null ? LessonFinalFile : null,
                    SubjectId = subject.SubjectID,
                    IsActive = true,
                    Order = createLessonDto.Order,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                 unitOfWork.GetRepository<Lesson, int>().AddAsync(lesson);
                var result = await unitOfWork.SaveChanges();
                if(result>0){
                    return "Lesson Added Successfully";
                }
                else throw new DatabaseException("Error in Adding Lesson");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Adding Lesson");
                throw;
            }

        }

        public Task<string> DeleteLesson(string lessonCode)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResponse<LessonShortResponseDto>> GetAllLessonsInSubject(string subjectCode)
        {
            throw new NotImplementedException();
        }

        public Task<LessonDetailsResponseDto> GetLessonByCode(string lessonCode)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateLesson(string lessonCode, UpdateLessonDto updateLessonDto)
        {
            throw new NotImplementedException();
        }
    }
}
