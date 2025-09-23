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
        public Task<string> AddLesson(string subjectCode, CreateLessonDto createLessonDto);
        public Task<string> UpdateLesson(string lessonCode, UpdateLessonDto updateLessonDto);
        public Task<string> DeleteLesson(string lessonCode);
        public Task<LessonDetailsResponseDto> GetLessonByCode(string lessonCode);
        public Task<PaginationResponse<LessonShortResponseDto>> GetAllLessonsInSubject(string subjectCode);
    }
}
