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
         Task<string> AddLesson(string subjectCode, CreateLessonDto createLessonDto,string email);
         
         Task<string> UpdateLesson(string lessonCode, UpdateLessonDto updateLessonDto,string email);
         
         Task<string> DeleteLesson(string lessonCode);
         
         Task<LessonDetailsResponseDto> GetLessonByCode(string lessonCode);

        Task<PaginationResponse<LessonShortResponseDto>> GetAllLessonsInSubject(string subjectCode, Subject_LessonFilteration filteration, string email, string Role);


         //Task<PaginationResponse<LessonForAdminDashboardDto>> GetAllforTeacher(string subjectCode, Subject_LessonFilteration filteration,string email);
        
         Task<string> UploadFile(string LeesonCode, UploadFileDto fileDto);

         Task<string> DeleteFile(string LessonCode, string email);



    }
}
