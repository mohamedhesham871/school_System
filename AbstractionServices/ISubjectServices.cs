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
        Task<string> AddSubjectAsync(CreateSubjectDto subjectDto);
        //Delete Subject
        Task<string> DeleteSubjectAsync(string subjectCode);
        //Get All Subjects with Grade
        Task<PaginationResponse<SubjectResponseShortDto>> GetAllSubjectAsync(SubjectFilteration subjectFilteration);
        //Get Subject By Id
        Task<SubjectResponseDto> GetSubjectByCodeAsync(string SubjectCode);

    }
}
