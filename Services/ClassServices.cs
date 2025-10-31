using AbstractionServices;
using AutoMapper;
using Domain.Contract;
using Domain.Exceptions;
using Domain.Models;
using Domain.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Shared.GradeDtos;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.IdentityDtos.Admin;
using Services.SpecificationsFile.Classes;
using Services.SpecificationsFile;
using Microsoft.IdentityModel.Tokens;

namespace Services
{
    public  class ClassServices(IUnitOfWork unitOfWork, ILogger<AppUsers> logger) :IClassServices
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<AppUsers> _logger = logger;

        public async Task<GenericResponseDto> AddClass(int GradeId, ClassCreateOrUpdate create)
        {
            try
            {
                //Validate Input
                var grade = await _unitOfWork.GetRepository<Grade, int>().GetByIdAsync(GradeId);
                if (grade is null)
                    throw new NotFoundException($"Grade with ID {GradeId} not found.");
                //check if Class Name IS Already Found 
                var IsFound = await _unitOfWork.GetRepository<ClassEntity, int>().CountAsync(new Specifications<ClassEntity>(s => s.ClassName == create.ClassName));
                if (IsFound >0)
                    throw new BadRequestException($"Class with Name {create.ClassName} is already exists.");
                //Create New Class
                var classEntity = new ClassEntity
                {
                    ClassName = create.ClassName,
                    GradeID = GradeId
                };
                _unitOfWork.GetRepository<ClassEntity, int>().AddAsync(classEntity);
                var res = await _unitOfWork.SaveChanges();
                if (res <= 0)
                    throw new BadRequestException("Failed to create new Class.");
                return new GenericResponseDto() { IsSuccess = true, Message = "Class Created Successfully" };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while Add New Class ");
                return new GenericResponseDto()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<GenericResponseDto> UpdateClass(string ClassCode, ClassCreateOrUpdate update)
        {
            try
            {
                //Validate Input
                if (string.IsNullOrEmpty(ClassCode))
                    throw new NullReferenceException("ClassCode cannot be null or empty.");

                var classEntity = await _unitOfWork.GetRepository<ClassEntity, string>().GetEntityWithCode<ClassEntity>(ClassCode);
                if (classEntity is null)
                    throw new NotFoundException($"Class with Code {ClassCode} not found.");

                //Update Class Data
                classEntity.ClassName = update.ClassName ?? classEntity.ClassName;
                classEntity.UpdateAt = DateTime.UtcNow;
                var res = await _unitOfWork.SaveChanges();
                if (res <= 0)
                    throw new BadRequestException("Failed to update Class.");
                return new GenericResponseDto()
                {
                    IsSuccess = true,
                    Message = "Class Updated Successfully"
                };



            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while Update Class ");
                return new GenericResponseDto()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<GenericResponseDto> DeleteClass(string ClassCode)
        {
            try
            {
                //Validate Input
                if (string.IsNullOrEmpty(ClassCode))
                    throw new NullReferenceException("ClassCode cannot be null or empty.");
                var classEntity = await _unitOfWork.GetRepository<ClassEntity, string>().GetEntityWithCode<ClassEntity>(ClassCode);
                if (classEntity is null)
                    throw new NotFoundException($"Class with Code {ClassCode} not found.");
                //Delete Class
                _unitOfWork.GetRepository<ClassEntity, string>().DeleteAsync(classEntity);
                var res = await _unitOfWork.SaveChanges();
                if (res <= 0)
                    throw new BadRequestException("Failed to delete Class.");
                return new GenericResponseDto() { IsSuccess = true, Message = "Class Deleted Successfully" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while Delete Class ");
                return new GenericResponseDto()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ClassDetialsResponseDto> classDetialsResponseDto(string ClassCode)
        {
            try
            {
                //Validate Input
                if (string.IsNullOrEmpty(ClassCode))
                    throw new NullReferenceException("ClassCode cannot be null or empty.");
                var spec = new ClassSpecification(ClassCode);
                var classEntity = await _unitOfWork.GetRepository<ClassEntity, string>().GetBySpecific(spec);

                if (classEntity is null)
                    throw new NotFoundException($"Class with Code {ClassCode} not found.");
                var Response = new ClassDetialsResponseDto()
                {
                    ClassCode = classEntity.Code,
                    ClassName = classEntity.ClassName,
                    GradeResponseShortDto = new GradeResponseShortDto()
                    {
                        GradeCode = classEntity.Grade.GradeCode,
                        GradeName = classEntity.Grade.GradeName,
                        AcademicYear = classEntity.Grade.AcademicYear
                    }
                };
                return Response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while Get Class Details By Code ");
                throw;
            }
        }

        public async Task<PaginationResponse<ClassDetialsResponseDto>> GetAllClasses(ClassFilteration filter)
        {
            try
            {
                var spec = new ClassSpecification(filter ,false);
                var classes= await _unitOfWork.GetRepository<ClassEntity,int>().GetByConditionAsync(spec);
                var count = new ClassSpecification(filter,true);
                var totalcount = await _unitOfWork.GetRepository<ClassEntity, int>().CountAsync(count);

                var paginationResponse = new PaginationResponse<ClassDetialsResponseDto>()
                {
                    Skip = filter.PageIndex,
                    Take = filter.PageSize,
                    Total = totalcount,
                    Data = classes.Select(c=>new ClassDetialsResponseDto()
                    {
                        ClassName = c.ClassName,
                        ClassCode=c.Code,
                        GradeResponseShortDto = new GradeResponseShortDto()
                        {
                            GradeCode=c.Grade.GradeCode,
                            GradeName=c.Grade.GradeName,
                            AcademicYear=c.Grade.AcademicYear
                        }

                    })
                };
                return paginationResponse;

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error While Try to retrieve All Classes");
                throw;
            }
        }
    }
}
