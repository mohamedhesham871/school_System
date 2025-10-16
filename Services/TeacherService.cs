using AbstractionServices;
using AutoMapper;
using Domain.Contract;
using Domain.Exceptions;
using Domain.Models;
using Domain.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Services.SpecificationsFile;
using Shared;
using Shared.IdentityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Services
{
    public class TeacherService(UserManager<AppUsers> _manager,
                                IMapper mapper,
                                IUnitOfWork repo
                                ) : ITeacherService
    {

        //Adding Teacher Async Method
        public async Task<string> AddTeacherAsync(NewTeacherDto teacherDto)
        {
            // 1) Pre-checks
            var existingByEmail = await _manager.FindByEmailAsync(teacherDto.Email);
            if (existingByEmail != null)
                throw new BadRequestException($"Email :{teacherDto.Email} is Already Found try Another Email");

            if (teacherDto.Password != teacherDto.ConfirmPassword)
                throw new BadRequestException("Password and Confirm Password do not match.");

            if (!Regex.IsMatch(teacherDto.PhoneNumber, @"^(010|011|012|015)[0-9]{8}$"))
                throw new BadRequestException($"Phone Number :{teacherDto.PhoneNumber} is Invalid");

            // 2) Prepare profile image
            var profilePath = "images/Default_Icone.png";
            if (teacherDto.ProfileImage != null && teacherDto.ProfileImage.Length > 0)
                profilePath = await UploadImageAsync(teacherDto.ProfileImage);

            // 3) Create Teacher (not AppUsers)
            var user = mapper.Map<Teacher>(teacherDto);
            user.ProfileImage = profilePath;
            user.Gender = teacherDto.gender.ToString();
            user.Status = teacherDto.Status.ToString();

            var createRes = await _manager.CreateAsync(user, teacherDto.Password);
            if (!createRes.Succeeded)
            {
                var errors = createRes.Errors.Select(e => e.Description);
                throw new ValidationErrorsExecption(errors);
            }

            await _manager.AddToRoleAsync(user, "Teacher");

            // 4) Add Class relations (after user exists)
            if (teacherDto.AssignedClasses != null && teacherDto.AssignedClasses.Count > 0)
            {
                foreach (var classId in teacherDto.AssignedClasses)
                {
                    var @class = await repo.GetRepository<StudentClass, int>().GetByIdAsync(classId)
                                 ?? throw new NotFoundException($"Class with Id :{classId} Not Found");

                    // ensure TeacherClass uses string TeacherId and int ClassID
                    var exists = await repo.GetRepository<TeacherClass, object>()
                       .ExistsAsync(new TeacherClassByIdsSpec(user.Id, classId));
                    if (exists)
                        throw new BadRequestException($"Class :{classId} is Already Assigned to this Teacher");
                    repo.GetRepository<TeacherClass, object>().AddAsync(new TeacherClass
                    {
                        TeacherId = user.Id,
                        ClassID = classId
                    });
                }
            }

            // 5) Add Subjects (set FK to Teacher)
            if (teacherDto.AssignedSubjects != null && teacherDto.AssignedSubjects.Count > 0)
            {
                foreach (var subjectId in teacherDto.AssignedSubjects)
                {
                    var subject = await repo.GetRepository<Subject, int>().GetByIdAsync(subjectId)
                                 ?? throw new NotFoundException($"Subject :{subjectId} Not Found");

                    if (!string.IsNullOrEmpty(subject.TeacherId))
                        throw new BadRequestException($"Subject :{subjectId} is Already Assigned by Another Teacher");

                    subject.TeacherId = user.Id; // ensure Subject.TeacherId : string?
                     repo.GetRepository<Subject, int>().UpdateAsync(subject);
                }
            }

            // 6) Save non-Identity changes if your UnitOfWork requires Save
            await repo.SaveChanges();

            return "Teacher Created Successfully";
        }
        //Updateing Teacher Async Method
        public async Task UpdateTeacherAsync(string teacherId, UpdateTeacherDto teacherDto)
        {
            try
            {
                var existingUser = await _manager.FindByIdAsync(teacherId);
                if (existingUser is null) throw new NotFoundException($"Teacher is not found");

                // Check if the user is actually a teacher
                if (existingUser is not Teacher existingTeacher)
                    throw new BadRequestException($"User with Id :{teacherId} is not a teacher");

                //check if number  is valid 
                if (Regex.IsMatch(teacherDto.PhoneNumber, @"^(010|011|012|015)[0-9]{8}$") == false)
                    throw new BadRequestException($"Phone Number :{teacherDto.PhoneNumber} is Invalid");

                //image
                #region image
                var UserPictureProfile = existingTeacher.ProfileImage ?? "images/Default_Icone.png";

                if (teacherDto.ProfileImage != null && teacherDto.ProfileImage.Length > 0)
                {
                    UserPictureProfile = await UploadImageAsync(teacherDto.ProfileImage);
                }
                #endregion

                // Update basic user properties
                if(teacherDto.UserName is not null)
                existingTeacher.UserName = teacherDto.UserName;
                if (teacherDto.FirstName is not null)
                existingTeacher.FirstName = teacherDto.FirstName;
                if (teacherDto.LastName is not null)
                    existingTeacher.LastName = teacherDto.LastName;
                if (teacherDto.PhoneNumber is not null)
                    existingTeacher.PhoneNumber = teacherDto.PhoneNumber;
                if (teacherDto.Address is not null)
                    existingTeacher.Address = teacherDto.Address; 
                    existingTeacher.ProfileImage = UserPictureProfile;
                // Update teacher-specific properties
                if (teacherDto.Specialization is not null)
                    existingTeacher.Specialization = teacherDto.Specialization;
                if (teacherDto.Status is not null)
                    existingTeacher.Status = teacherDto.Status.ToString();

                // Update assigned classes and subjects (replace existing lists)
                if (teacherDto.AssignedClasses != null)
                {
                    existingTeacher.TeacherClasses ??= new List<TeacherClass>();
                    foreach (var classId in teacherDto.AssignedClasses)
                    {
                        var StudentClass = await repo.GetRepository<StudentClass, int>().GetByIdAsync(classId);
                        if (StudentClass == null) throw new NotFoundException($"Class with Id :{classId} Not Found");
                       
                        //check for duplicate subjects
                        if (existingTeacher.TeacherClasses != null && existingTeacher.TeacherClasses.Any(t=>t.ClassID==classId))
                            throw new BadRequestException($"Class :{classId} is Already Assigned to this Teacher");
                        //Create new relation
                        var teacherClass = new TeacherClass
                        {
                            TeacherId = existingTeacher.Id,
                            ClassID = classId
                        };
                        existingTeacher.TeacherClasses?.Add(teacherClass);
                    }
                }
                
                if (teacherDto.AssignedSubjects != null)
                {
                    foreach (var item in teacherDto.AssignedSubjects) // Fixed: was using AssignedClasses
                    {
                        //check if it subject is Already assigned by Another Teacher
                        var SubjectAssigned = await repo.GetRepository<Subject, int>().GetByIdAsync(item);
                        if (SubjectAssigned.TeacherId != null)
                            throw new BadRequestException($"Subject :{item} is Already Assigned by Another Teacher");
                        //check for duplicate subjects
                        if (existingTeacher.Subjects != null && existingTeacher.Subjects.Contains(SubjectAssigned))
                            throw new BadRequestException($"Subject :{item} is Already Assigned to this Teacher");
                      
                        existingTeacher.Subjects?.Add(SubjectAssigned);
                    }
                }

                // Update timestamp
                existingTeacher.UpdatedAt = DateTime.UtcNow;

                var res = await _manager.UpdateAsync(existingTeacher);
                if (!res.Succeeded)
                {
                    var errors = res.Errors.Select(e => e.Description);
                    throw new ValidationErrorsExecption(errors);
                }
            }
            catch(Exception)
            {
                throw new Exception("Something went wrong while updating the teacher.");
            }
        }
        //Deleting Teacher Async Method 
        public async Task<bool> DeleteTeacherAsync(string teacherId)
        {
            var user = await _manager.FindByIdAsync(teacherId);
            if (user == null) throw new NotFoundException($"Teacher with Id :{teacherId} is not found");
            
            // Check if the user is actually a teacher
            if (user is not Teacher)
                throw new BadRequestException($"User with Id :{teacherId} is not a teacher");
                
            var res = await _manager.DeleteAsync(user);
            if (!res.Succeeded)
            {
                var errors = res.Errors.Select(e => e.Description);
                throw new ValidationErrorsExecption( errors);
            }
            return true;
        }
        //Get User By ID
        public async Task<TeacherResultDto?> GetTeacherByIdAsync(string teacherId)
        {
            var user = await _manager.FindByIdAsync(teacherId);
            if (user == null) throw new NotFoundException($"Teacher with Id :{teacherId} is not found");
            
            // Check if the user is actually a teacher
            if (user is not Teacher)
                throw new BadRequestException($"User with Id :{teacherId} is not a teacher");
                
            var res = mapper.Map<TeacherResultDto>(user);
            return res;
        }
        //Find User By Email
        public async Task<TeacherResultDto?> FindTeacherByEmailAsync(string email)
        {
            var user = await _manager.FindByEmailAsync(email);
            if (user == null) throw new NotFoundException($"Teacher with email :{email} is not found");
            
            // Check if the user is actually a teacher
            if (user is not Teacher)
                throw new BadRequestException($"User with email :{email} is not a teacher");
                
            var res = mapper.Map<TeacherResultDto>(user);
            return res;
        }

        public async Task<PaginationResponse<TeacherResultDto>> GetAllTeachersAsync(TeacherFilteration teacherFilteration)
        {
            var teachersWithSpec = new TeacherSpecificationWithClassAndSubjects(teacherFilteration);
            var teachers = await repo.GetRepository<Teacher, Guid>().GetByConditionAsync(teachersWithSpec);

            var totalCount = new TeacherCountSpecifications(teacherFilteration);
            var count = await repo.GetRepository<Teacher, Guid>().CountAsync(totalCount);

            var DataMapped = mapper.Map<IEnumerable<TeacherResultDto>>(teachers);
            var PaginationMetaData = new PaginationResponse<TeacherResultDto>
            {
                Skip = teacherFilteration.PageIndex,
                Take = teacherFilteration.PageSize,
                Total = count,
                Data = DataMapped
            };
            return PaginationMetaData;
        }



        private async Task<string> UploadImageAsync(IFormFile imageFile)
        {
            var images = "images/Default_Icone.png";

            if (imageFile != null && imageFile.Length > 0)
            {
                var UploadImage = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                var fileName = $"{Guid.NewGuid()}_{imageFile.FileName}";
                var filePath = Path.Combine(UploadImage, fileName);

                using (var Stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(Stream);
                }
                images = $"/images/{fileName}"; // Set the new profile picture path
            }
            return images;
        }
    }
}