using AbstractionServices;
using Domain.Contract;
using Domain.Exceptions;
using Domain.Models;
using Domain.Models.User;
using Microsoft.Extensions.Logging;
using Services.SpecificationsFile;
using Services.SpecificationsFile.Subjects;
using Shared;
using Shared.GradeDtos;
using Shared.IdentityDtos;
using Shared.SubjectDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class SubjectServices(IUnitOfWork unitOfWork ,ILogger<SubjectServices> _logger) : ISubjectServices
    {
        //Add new Subject
        public async Task<GenericResponseDto> AddSubjectAsync(CreateSubjectDto subjectDto,int GradeId)
        {

            try
            {
                //check if Subject already exists
                var subjectExists = await unitOfWork.GetRepository<Subject, int>().ExistsAsync(new Specifications<Subject>(s => s.Code == subjectDto.SubjectCode));
                if (subjectExists)
                {
                    throw new BadRequestException("Subject Already Exists Please Enter Another Code");
                }
                //check if Grade Exists
                var grade = await unitOfWork.GetRepository<Grade, int>().GetByIdAsync(GradeId);
                if (grade is null) throw new NotFoundException("Grade Not Found ,Please Enter Valid Grade");
                //check if Teacher Exists if not null
                #region New Changes
                //if (subjectDto.TeacherId is not null)
                //{
                //    var teacherExists = await unitOfWork.GetRepository<Teacher, string>().GetByIdAsync(subjectDto.TeacherId);
                //    if (teacherExists is null) throw new NotFoundException("Teacher Not Found ,Please Enter Valid TeacherId");
                //} 
                #endregion
                var subject = new Subject
                {
                    Code = subjectDto.SubjectCode,
                    SubjectName = subjectDto.SubjectName,
                    Description = subjectDto.Description,
                    GradeID = GradeId
                };
                //Add Subject To DataBase
                unitOfWork.GetRepository<Subject, int>().AddAsync(subject);

                var result = await unitOfWork.SaveChanges();
                if (result > 0)
                {
                    return new GenericResponseDto(isSuccess: true,
                        message: "Subject Added Successfully");

                }
                else
                {
                    throw new DatabaseException("Error in Adding Subject");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Adding Subject");
                return new GenericResponseDto(false, "Error Happen while Add New Subject");
            }
        }
        //Delete Subject
        public async Task<GenericResponseDto> DeleteSubjectAsync(string subjectCode ,int GradeId)
        {
            try
            {
                // check if Grade Exists
                var Grade = await unitOfWork.GetRepository<Grade, int>().GetByIdAsync(GradeId);
                if (Grade is null) throw new NotFoundException("Grade Not Found ,Please Enter Valid Grade");
                // check if Grade Contain this Subject
                var subjectInGrade = await unitOfWork.GetRepository<Subject, int>().ExistsAsync(new Specifications<Subject>(s => s.Code == subjectCode && s.GradeID == GradeId));
                if(subjectInGrade is false) throw new UnauthorizedAccessException("Can't Delete From Another Grade !!");
                //check if Subject Exists
                var subject = await unitOfWork.GetRepository<Subject, int>().GetEntityWithCode<Subject>(subjectCode);
                if (subject is null) throw new NotFoundException("Subject Not Found");

                //Delete Subject Of Grade
                unitOfWork.GetRepository<Subject, int>().DeleteAsync(subject);
                //Save Changes
                var result = await unitOfWork.SaveChanges();

                if (result > 0) return new GenericResponseDto(isSuccess: true,
                        message: "Subject Deleted Successfully");
                else
                    throw new DatabaseException("Error in Deleting Subject");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in Deleting Subject");
                return new GenericResponseDto(false, "Error Happen while Deleting Subject");
            }
        }

        //Update Subject Name Or Description

        public async Task<GenericResponseDto> UPdateSubjectAsync(UpdateSubjectDto updateSubjectDto, string subjectCode,int gradeId)
        {
            try
            {
                var Grade = await unitOfWork.GetRepository<Grade, int>().GetByIdAsync(gradeId);
                if (Grade is null) throw new NotFoundException("Grade Not Found ,Please Enter Valid Grade");
                // check if Grade Contain this Subject
                var subjectInGrade = await unitOfWork.GetRepository<Subject, int>().ExistsAsync(new Specifications<Subject>(s => s.Code == subjectCode && s.GradeID == gradeId));
                if (subjectInGrade is false) throw new UnauthorizedAccessException("Can't Update From Another Grade !!");

                //cheack if Subject Exists
                var subject = await unitOfWork.GetRepository<Subject, int>().GetEntityWithCode<Subject>(subjectCode);
                if (subject is null) throw new NotFoundException("Subject Not Found");
                //Update Subject Info
                subject.SubjectName = updateSubjectDto.SubjectName ?? subject.SubjectName;
                subject.Description = updateSubjectDto.Description ?? subject.Description;
                subject.UpdatedAt = DateTime.UtcNow;
                unitOfWork.GetRepository<Subject, int>().UpdateAsync(subject);
                //Save Changes
                var result = await unitOfWork.SaveChanges();
                if (result > 0) return new GenericResponseDto(isSuccess: true,
                        message: "Subject Updated Successfully");
                else
                    throw new DatabaseException("Error in Updating Subject");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Updating Subject");
                return new GenericResponseDto(false, "Error Happen while Updating Subject");
            }
        }

        //Get Subject By [Code]
        public async Task<SubjectResponseDetailsDto> GetSubjectByCodeAsync(string SubjectCode)
        {
            var Spec = new SubjectSpecifications(SubjectCode);

            var subject = await unitOfWork.GetRepository<Subject, int>().GetBySpecific(Spec);
            if (subject is null) throw new NotFoundException($"Subject with code '{SubjectCode}' was not found.");

            var subjectDto = new SubjectResponseDetailsDto
            {
                SubjectCode = subject.Code,
                SubjectName = subject.SubjectName,
                Description = subject.Description,
                //Grade Info
                Grade = new GradeResponseShortDto
                {
                    GradeCode = subject.Grade.GradeCode,
                    GradeName = subject.Grade.GradeName,
                    AcademicYear = subject.Grade.AcademicYear

                },
                //Teacher Info
                Teacher = subject.Teacher == null ? null : new TeacherShortResponseDto
                {
                    userId = subject.Teacher.Id,
                    FirstName = subject.Teacher.FirstName,
                    UserName = subject.Teacher.UserName,
                    Email = subject.Teacher.Email,
                    Specialization = subject.Teacher.Specialization,
                    status =subject.Teacher.Status,
                    className =subject.Teacher.TeacherClasses.Select(s=>s.Class.ClassName).ToList(),
                    subjectAssignName=subject.Teacher.Subjects.Select(s=>s.SubjectName).ToList()

                },
                //Lessons Info
                Lessons = subject.Lessons.Select(l => new LessonShortResponseDto
                {
                    LessonCode = l.Code,
                    Title = l.Title
                }).ToList()
            };
            return subjectDto;
        }
        // Get All subject as pagination   
        public async Task<PaginationResponse<SubjectResponseShortDto>> GetAllSubjectAsync(Subject_LessonFilteration subjectFilteration)
        {
            var Spec = new SubjectSpecifications(subjectFilteration, false);
            var subjects = await unitOfWork.GetRepository<Subject, int>().GetByConditionAsync(Spec);
            var CountAllSubjects = new SubjectSpecifications(subjectFilteration, true);
            var totalCount = await unitOfWork.GetRepository<Subject, int>().CountAsync(CountAllSubjects);
            var subjectDtos = subjects.Select(s => new SubjectResponseShortDto
            {
                SubjectCode = s.Code,
                SubjectName = s.SubjectName,
                Description = s.Description,
            });
            var paginationResponse = new PaginationResponse<SubjectResponseShortDto>
            {
                Data = subjectDtos,
                Total = totalCount,
                Skip = subjectFilteration.PageIndex,
                Take = subjectFilteration.PageSize
            };
            return paginationResponse;
        }

        // Get All Subject Based On Grade Id
        public async Task<PaginationResponse<SubjectResponseShortDto>> GetAllSubjectByGradeIdAsync(Subject_LessonFilteration filter, int GradeId)
        {
            try
            {
                var GradeExists = await unitOfWork.GetRepository<Grade, int>().GetByIdAsync(GradeId);
                if (GradeExists is null) throw new NotFoundException("Grade Not Found ,Please Enter Valid Grade");
                var Spec = new SubjectSpecifications(filter, false, GradeId);
                var subjects = await unitOfWork.GetRepository<Subject, int>().GetByConditionAsync(Spec);
                var CountAllSubjects = new SubjectSpecifications(filter, true, GradeId);
                var totalCount = await unitOfWork.GetRepository<Subject, int>().CountAsync(CountAllSubjects);
                var subjectDtos = subjects.Select(s => new SubjectResponseShortDto
                {
                    SubjectCode = s.Code,
                    SubjectName = s.SubjectName,
                    Description = s.Description,
                });
                var paginationResponse = new PaginationResponse<SubjectResponseShortDto>
                {
                    Data = subjectDtos,
                    Total = totalCount,
                    Skip = filter.PageIndex,
                    Take = filter.PageSize
                };
                return paginationResponse;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Getting Subjects By Grade Id");
                throw new DatabaseException("Error Happen while Getting Subjects By Grade Id");
            }
        }
       
    }
}
