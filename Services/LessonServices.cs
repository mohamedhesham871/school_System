using AbstractionServices;
using AutoMapper;
using Domain.Contract;
using Domain.Exceptions;
using Domain.Models;
using Domain.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Services.SpecificationsFile;
using Services.SpecificationsFile.Lessons;
using Shared;
using Shared.Lesson_Dto;
using Shared.QuizDto;
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
    public class LessonServices(IUnitOfWork unitOfWork,ILogger<LessonServices>_logger,IMapper _mapper,UserManager<AppUsers> userManager ) : ILessonServices
    {
        //Create New Lesson
        public async Task<GenericResponseDto> AddLesson(string subjectCode, CreateLessonDto createLessonDto,string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || (string.IsNullOrEmpty(subjectCode))) throw new NullReferenceException("Eamil OR subject code is not Valid");
                //1- check on Subject exist

                //check if Teacher Have Auth to Add New Lesson 
                // Get Subject using subjectCode and then Get Teacher Using email and check if Teacher Authorize


                var subject = await unitOfWork.GetRepository<Subject, int>().GetEntityWithCode<Subject>(subjectCode);
                if (subject is null) throw new NotFoundException($"Subject is Not Found Please Try Valid Subject");
                
                var Teacher = await userManager.FindByEmailAsync(email);
                if (subject.TeacherId != Teacher?.Id) throw new UnauthorizedAccessException("Teacher UnAuthorize Access to Add in this Subject");

                var lesson = new Lesson()
                {
                    Title = createLessonDto.Title,
                    Description = createLessonDto.Description,
                    SubjectId = subject.SubjectID,
                    IsActive = true,
                    Order = createLessonDto.Order,
                };
                unitOfWork.GetRepository<Lesson, int>().AddAsync(lesson);
                var result = await unitOfWork.SaveChanges();
                if (result > 0)
                {
                    _logger.LogInformation("User {Email} added a new lesson {LessonTitle} to subject {SubjectName} at {Time}", email, createLessonDto.Title, subject.SubjectName, DateTime.UtcNow.ToLocalTime());
                    return new GenericResponseDto() { IsSuccess = true, Message = "Lesson Added Successfully" };
                }
                else throw new DatabaseException("Error in Adding Lesson");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Adding Lesson");
                return  new GenericResponseDto() { IsSuccess = false, Message = ex.Message };
            }

        }

        //Delete Lesson
        public async Task<GenericResponseDto> DeleteLesson(string lessonCode,string email)
        {
            try
            {
                //check on Lesson exist
                var Lesson = await unitOfWork.GetRepository<Lesson, int>().GetEntityWithCode<Lesson>(lessonCode);

                if (Lesson is null) throw new NotFoundException("Lesson Not Found Please Try Valid");

                //Delete Lesson
                unitOfWork.GetRepository<Lesson, int>().DeleteAsync(Lesson);
                var result = await unitOfWork.SaveChanges();
                if (result > 0)
                {
                    _logger.LogInformation("Admin email :{email} Delete lesson {LessonCode}  at {Time}", email, lessonCode,  DateTime.UtcNow.ToLocalTime());

                    return new GenericResponseDto() { IsSuccess = true, Message = "Deleting Lesson Successfully ..!" };
                }
                else throw new DatabaseException("Error in Deleting Lesson");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Deleting Lesson");
                return new GenericResponseDto() { IsSuccess = false, Message = ex.Message };
            }


        }
   
        //Update Lesson
        public async Task<GenericResponseDto> UpdateLesson(string lessonCode, UpdateLessonDto updateLessonDto,string userId)
        {
            try
            {
                // input validation
                if (string.IsNullOrWhiteSpace(lessonCode)||string.IsNullOrEmpty(userId))
                    throw new NullRefrenceException($"Lesson code Or Email cannot be null or empty {nameof(lessonCode)}");

                //We want to check if Teacher Have Auth to update this Lesson
                //1- Get Teacher 
                //2- Get Lesson Using lessonCode [check if Lesson Exist]
                //3- Get Subjet using Subject ID and then Compare Subject Teacher id with Teacher Who try to Update

                //Get Teacher [Already we have Teacher Id form Token ]


                //check on Lesson if Exist
                var Spec = new LessonWithSubjectSpec(lessonCode);
                var lesson = await unitOfWork.GetRepository<Lesson, int>().GetBySpecific(Spec);
                if (lesson is null) throw new NotFoundException("Lesson Not Found Please Try Valid");

               
                //Get Subject using Id
                if (userId != lesson.Subject.TeacherId) throw new UnauthorizedAccessException("Teacher UnAuthorize to make upadate to this Lesson ");
                
                //map the dto to the entity & then  update data
             
                //Updateing Data 
                lesson.Title = updateLessonDto.Title ?? lesson.Title;
                lesson.IsActive = updateLessonDto.IsActive ?? lesson.IsActive;
                lesson.Description = updateLessonDto.Description ?? lesson.Description;
                lesson.Order = updateLessonDto.Order ?? lesson.Order;
                lesson.UpdatedAt = DateTime.UtcNow.ToLocalTime();

                unitOfWork.GetRepository<Lesson, int>().UpdateAsync(lesson);
                
                var result = await unitOfWork.SaveChanges();
                if (result > 0)
                {
                    _logger.LogInformation("User {userId} updated lesson {LessonCode} at {Time}", userId, lessonCode, DateTime.UtcNow.ToLocalTime());
                    
                    return new GenericResponseDto() { IsSuccess = true, Message = "Lesson Updated Successfully" };
                }
                else throw new DatabaseException("Error in Updating Lesson");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Updating Lesson");
                return new GenericResponseDto() { IsSuccess = false, Message = ex.Message };
            }
        }

        //Upload File 
        public async Task<GenericResponseDto> UploadFile(string lessonCode, UploadFileDto fileDto ,string userId)
        {
            try
            {
                //Check if Lesson is Exist 
                
                var spec= new LessonWithSubjectSpec(lessonCode);
                var Lesson = await unitOfWork.GetRepository<Lesson, int>().GetBySpecific(spec);

                if (Lesson is null) throw new NotFoundException("Lesson Not Found Please Try Valid");

                if (Lesson.Subject.TeacherId != userId) throw new UnauthorizedAccessException("Teacher UnAuthorize to Access to this Function");
              
                if (Lesson.MaterialUrl is not null)
                    throw new BadRequestException("Can't Upload File you Already have One ,Delete File to Upload New");

                //if MatrialUrl is not null check if its valid url
                var LessonFinalFile = string.Empty;
                if (fileDto.Matrial is not null)
                {
                    //1- check on Extension
                    var Extention = Path.GetExtension(fileDto.Matrial.FileName).ToLowerInvariant();
                    if (Extention != ".pdf" && Extention != ".docx" && Extention != ".pptx" && Extention != ".mp4")
                    {
                        throw new InvalidOperationException("Invalid File Extension. Only PDF, DOCX, PPTX, and MP4 are allowed.");
                    }
                    //2- check on MIME type
                    var LessonFileType = fileDto.Matrial.ContentType;
                    if (LessonFileType != "application/pdf" &&   // PDF
                        LessonFileType != "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && // DOCX
                        LessonFileType != "application/vnd.openxmlformats-officedocument.presentationml.presentation" && // PPTX
                        LessonFileType != "video/mp4")// MP4
                    {
                        throw new InvalidOperationException("Invalid File Type. Only PDF, DOCX, PPTX, and MP4 are allowed.");
                    }
                    //3- save file to server
                    var UploadFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/FolderFile/Lessons");
                    var fileName = $"{Guid.NewGuid()}_{fileDto.Matrial.FileName}";
                    var filePath = Path.Combine(UploadFile, fileName);

                    using (var Stream = new FileStream(filePath, FileMode.Create))
                    {
                        await fileDto.Matrial.CopyToAsync(Stream);
                    }
                    LessonFinalFile = $"/FolderFile/Lessons/{fileName}";
                    //4- Save File Path to Database
                    Lesson.MaterialUrl = LessonFinalFile;
                   
                }
                unitOfWork.GetRepository<Lesson, int>().UpdateAsync(Lesson);
                var res = await unitOfWork.SaveChanges();

                if (res > 0)
                {
                    _logger.LogInformation("User with Id {userId} File uploaded for lesson {LessonCode} at {Time}", userId,lessonCode, DateTime.UtcNow.ToLocalTime());
                    return new GenericResponseDto() { IsSuccess = true, Message = "File Uploaded Successfully" };
                }
                else
                    throw new DatabaseException("Error in Uploading File");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Uploading File for lesson {LessonCode}", lessonCode);
                return new GenericResponseDto() { IsSuccess = false, Message = ex.Message };
            }
        }
       // Delete File
        public async Task<GenericResponseDto> DeleteFile(string LessonCode,string userId)
        {
            try
            {
                if (LessonCode is null ) throw new NullReferenceException("Lesson  is not Valid");


                var spec = new LessonWithSubjectSpec(LessonCode);
                var lesson = await unitOfWork.GetRepository<Lesson, int>().GetBySpecific(spec);
                if (lesson is null) throw new NotFoundException("Not Found Lesson With that code to Delete");


                if (lesson.Subject.TeacherId != userId) throw new UnauthorizedAccessException("Teacher UnAuthorize to Access to this Function");

                lesson.MaterialUrl = null;

                unitOfWork.GetRepository<Lesson, int>().UpdateAsync(lesson);
                var res = await unitOfWork.SaveChanges();
                if (res > 0)
                {
                    _logger.LogInformation("User with Id :{userId} File deleted for lesson {LessonCode} at {Time}",userId, LessonCode, DateTime.UtcNow.ToLocalTime());
                    
                    return new GenericResponseDto() { IsSuccess = true, Message = "File Deleted Successfully" };
                }
                else
                    throw new BadRequestException( "An Error While Delete File");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Deleting File for lesson {LessonCode}", LessonCode);
                return new GenericResponseDto() { IsSuccess = false, Message = ex.Message };
            }

        }


        //Get All Lessons In Subject
        public async Task<PaginationResponse<LessonShortResponseDto>> GetAllLessonsInSubject(string subjectCode, Subject_LessonFilteration filteration,string email,string Role)
        {
            //check input validation
            if (string.IsNullOrWhiteSpace(subjectCode))
                throw new NullRefrenceException($"Subject code cannot be null or empty {nameof(subjectCode)}");

            if (email is null) throw new NullReferenceException("email is Invalid");
            var subeject = await unitOfWork.GetRepository<Subject, int>().GetEntityWithCode<Subject>(subjectCode);
            if (subeject is null) throw new NotFoundException("Subject Not Found Please Try Valid");

            var CountAllSubjects = new LessonSpecCount();
            var spec = new LessonInSubjectSpecification();
            if (Role=="Teacher")
            {
                var teacher = await userManager.FindByEmailAsync(email);
                if (string.IsNullOrWhiteSpace(subjectCode))
                    throw new NullRefrenceException($"Subject code cannot be null or empty {nameof(subjectCode)}");
                if (subeject.TeacherId != teacher.Id) throw new UnauthorizedAccessException(message: "Teacher UnAuthorize Access to this subject");

                spec = new LessonInSubjectSpecification(filteration, subeject.SubjectID);
                CountAllSubjects =new LessonSpecCount(filteration, subeject.SubjectID);
            }
            else
            {
                spec = new LessonInSubjectSpecification(filteration, subeject.SubjectID, true);
                CountAllSubjects = new LessonSpecCount(filteration, subeject.SubjectID, true);

            }


            var Lessons = await unitOfWork.GetRepository<Lesson, int>().GetByConditionAsync(spec);
            var totalCount = await unitOfWork.GetRepository<Lesson, int>().CountAsync(CountAllSubjects);
           
            var LessonDto = Lessons.Select(s => new LessonShortResponseDto
            {
                LessonCode = s.Code,
                Title = s.Title,
                IsActive=s.IsActive

            });
            var paginationResponse = new PaginationResponse<LessonShortResponseDto>
            {
                Data = LessonDto,
                Total = totalCount,
                Skip = filteration.PageIndex,
                Take = filteration.PageSize
            };
            return paginationResponse;

        }
      

        public async Task<LessonDetailsResponseDto> GetLessonByCode(string lessonCode)
        {
            if (lessonCode is null) throw new NullRefrenceException("Lesson Code Can't be Empty");
            var spec = new LessonInSubjectSpecification(lessonCode);
            var lesson = await unitOfWork.GetRepository<Lesson, int>().GetBySpecific(spec);
            if (lesson is null)
                throw new NotFoundException("Lesson not found. Please try a valid code.");

            var ResponseLesson = new LessonDetailsResponseDto()
            {
                LessonCode = lesson.Code,
                Title = lesson.Title,
                Description = lesson.Description,
                MaterialUrl = lesson.MaterialUrl,
                Order = lesson.Order,
                Subject = new SubjectResponseShortDto
                {
                    SubjectCode = lesson.Subject.Code,
                    SubjectName = lesson.Subject.SubjectName,
                    Description = lesson.Subject.Description
                },
                Quiz = lesson.quiz is not null ? new QuizShort
                {
                    QuizCode = lesson.quiz.Code,
                    Title = lesson.quiz.Title,
                    TotalMarks = lesson.quiz.TotalMarks,
                } : null


            };
            return ResponseLesson;
        }

       
    }
}
