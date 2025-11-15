using Shared;
using Shared.Lesson_Dto;
using Shared.SubjectDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractionServices
{
    public interface ILessonServices
    {
         Task<GenericResponseDto> AddLesson(string subjectCode, CreateLessonDto createLessonDto,string email);
         
         Task<GenericResponseDto> UpdateLesson(string lessonCode, UpdateLessonDto updateLessonDto,string email);
         
         Task<GenericResponseDto> DeleteLesson(string lessonCode,string email);
         
         Task<LessonDetailsResponseDto> GetLessonByCode(string lessonCode);

        Task<PaginationResponse<LessonShortResponseDto>> GetAllLessonsInSubject(string subjectCode, Subject_LessonFilteration filteration, string email, string Role);


         //Task<PaginationResponse<LessonForAdminDashboardDto>> GetAllforTeacher(string subjectCode, Subject_LessonFilteration filteration,string email);
        
         Task<GenericResponseDto> UploadFile(string LeesonCode, UploadFileDto fileDto,string email);

         Task<GenericResponseDto> DeleteFile(string LessonCode, string email);



    }
}
