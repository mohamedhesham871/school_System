using Shared.IdentityDtos.Admin;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractionServices
{
    public interface IClassServices
    {
        Task<PaginationResponse<ClassDetialsResponseDto>> GetAllClasses(ClassFilteration filter);
        Task<GenericResponseDto> AddClass(int GradeId, ClassCreateOrUpdate create);
        Task<GenericResponseDto> UpdateClass(string ClassCode, ClassCreateOrUpdate update);
        Task<GenericResponseDto> DeleteClass(string ClassCode);
        Task<ClassDetialsResponseDto> classDetialsResponseDto(string ClassCode);

    }
}
