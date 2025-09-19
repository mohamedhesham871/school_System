using Shared;
using Shared.IdentityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractionServices
{
    public  interface ITeacherService
    {
        Task<string> AddTeacherAsync(NewTeacherDto teacher);
        Task UpdateTeacherAsync(string teacherId, UpdateTeacherDto teacher);
        Task<bool> DeleteTeacherAsync(string teacherId);
        Task<PaginationResponse<TeacherResultDto>> GetAllTeachersAsync(TeacherFilteration teacherFilteration);
        Task<TeacherResultDto?> GetTeacherByIdAsync(string teacherId);
        Task<TeacherResultDto?> FindTeacherByEmailAsync(string email);

    }
}
