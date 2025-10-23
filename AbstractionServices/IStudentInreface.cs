using Shared;
using Shared.IdentityDtos;
using Shared.SubjectDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractionServices
{
    public interface IStudentInreface
    {
        Task<PaginationResponse<SubjectResponseShortDto>> GetAllSubjectInRolled(string UserId);
        
    }
}
/*
 GET /Api/Student/Dashboard` - Get student dashboard
- `GET /Api/Student/Profile` - Get student profile
- `PUT /Api/Student/Profile` - Update student profile
- `GET /Api/Student/MySubjects` - Get enrolled subjects
- `GET /Api/Student/Grades` - Get all grades/results*/