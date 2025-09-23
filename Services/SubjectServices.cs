using AbstractionServices;
using Domain.Contract;
using Domain.Exceptions;
using Domain.Models;
using Domain.Models.User;
using Microsoft.Extensions.Logging;
using Services.SpecificationsFile;
using Shared;
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
        public async Task<string> AddSubjectAsync(CreateSubjectDto subjectDto)
        {
            
            try
            {
                //check if Subject already exists
                var subjectExists = await unitOfWork.GetRepository<Subject, int>().ExistsAsync(new SubjectDuplicationCheck(subjectDto.SubjectCode));
                if (subjectExists)
                {
                    throw new BadRequestException("Subject Already Exists Please Enter Another Code");
                }
                //check if Grade Exists
                var grade = await unitOfWork.GetRepository<Grade, int>().GetByIdAsync(subjectDto.GradeID);
                if (grade is  null) throw new NotFoundException("Grade Not Found ,Please Enter Valid Grade");
                //check if Teacher Exists if not null
                if (subjectDto.TeacherId is not null)
                {
                    var teacherExists = await unitOfWork.GetRepository<Teacher, string>().GetByIdAsync(subjectDto.TeacherId);
                    if (teacherExists is null) throw new NotFoundException("Teacher Not Found ,Please Enter Valid TeacherId");
                }
                var subject = new Subject
                {
                    Code = subjectDto.SubjectCode,
                    SubjectName = subjectDto.SubjectName,
                    Description = subjectDto.Description,
                    GradeID = subjectDto.GradeID,
                    TeacherId = subjectDto.TeacherId

                };
                  unitOfWork.GetRepository<Subject, int>().AddAsync(subject);
                var result = await unitOfWork.SaveChanges();
                if (result > 0) return "Subject Added Successfully";
                throw new DatabaseException("Error in Adding Subject");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in Adding Subject");
                throw;

            }
        }
        //Delete Subject
        public async Task<string> DeleteSubjectAsync(string subjectCode)
        {
            //cheack if Subject Exists
            var eixts = await  unitOfWork.GetRepository<Subject, int>().ExistsAsync(new SubjectDuplicationCheck(subjectCode));
            if (eixts is false) throw new NotFoundException("Subject Not Found");
            var subject = await unitOfWork.GetRepository<Subject, int>().GetByIdAsyncSpecific(new SubjectDuplicationCheck(subjectCode));
             unitOfWork.GetRepository<Subject, int>().DeleteAsync(subject);
            var result = await unitOfWork.SaveChanges();
            if (result > 0) return "Subject Deleted Successfully";
            throw new DatabaseException("Error in Deleting Subject");
        }

        public async Task<PaginationResponse<SubjectResponseShortDto>> GetAllSubjectAsync(SubjectFilteration subjectFilteration)
        {
           var Spec = new SubjectSpecificationWithGradeAndLessonAndTeacher(subjectFilteration);
            var subjects = await unitOfWork.GetRepository<Subject, int>().GetByConditionAsync(Spec);
            var CountAllSubjects = new SubjectSpecificationCount(subjectFilteration);
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
        //Get Subject By Id[Code]
        public async Task<SubjectResponseDto> GetSubjectByCodeAsync(string SubjectCode)
        {
            var Spec = new SubjectSpecificationWithGradeAndLessonAndTeacher(SubjectCode);

            var subject = await unitOfWork.GetRepository<Subject, int>().GetByIdAsyncSpecific(Spec);
            if (subject is null) throw new NotFoundException($"Subject with code '{SubjectCode}' was not found.");
            var subjectDto = new SubjectResponseDto
            {
                SubjectCode = subject.Code,
                SubjectName = subject.SubjectName,
                Description = subject.Description,
                Grade = new Shared.GradeDtos.GradeResponseShortDto
                {
                    GradeCode = subject.Grade.GradeCode,
                    GradeName = subject.Grade.GradeName,
                    AcademicYear = subject.Grade.AcademicYear

                },
                Teacher = subject.Teacher == null ? null : new TeacherShortResponse
                {
                    FirstName = subject.Teacher.FirstName,
                    UserName = subject.Teacher.UserName,
                    Email = subject.Teacher.Email,
                    Specialization = subject.Teacher.Specialization
                },
                Lessons =  subject.Lessons.Select(l => new LessonShortResponseDto
                {
                    LessonCode = l.Code,
                    Title = l.Title
                }).ToList()
            };
            return subjectDto;
        }
    }
}
