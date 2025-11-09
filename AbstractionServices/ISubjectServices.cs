using Shared;
using Shared.SubjectDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractionServices
{
    public interface ISubjectServices
    {
        //Add Subject
        Task<GenericResponseDto> AddSubjectAsync(CreateSubjectDto subjectDto ,int GradeId);
        //Delete Subject
        Task<GenericResponseDto> DeleteSubjectAsync(string subjectCode, int GradeId);
        //Update Subject
        Task<GenericResponseDto> UPdateSubjectAsync(UpdateSubjectDto updateSubjectDto, string subjectCode,int gradeId);

        //Get All Subjects with Grade
        Task<PaginationResponse<SubjectResponseShortDto>> GetAllSubjectAsync(Subject_LessonFilteration subjectFilteration);
        //Get Subject By Id
        Task<SubjectResponseDetailsDto> GetSubjectByCodeAsync(string SubjectCode);
        //Get All Subject Basesd On Grade Id
        Task<PaginationResponse<SubjectResponseShortDto>> GetAllSubjectByGradeIdAsync(Subject_LessonFilteration filter,int GradeId);
    }
}
