using AbstractionServices;
using AutoMapper;
using Domain.Contract;
using Domain.Exceptions;
using Domain.Models;
using Domain.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Services.SpecificationsFile.Teachers;
using Shared;
using Shared.IdentityDtos;
using Shared.IdentityDtos.Admin;
using Shared.SubjectDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Services
{
    public class AdminServices(IUnitOfWork unitOfWork ,UserManager<AppUsers> userManager,
                               IMapper mapper, ILogger<AppUsers> logger) : IAdminServices
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly UserManager<AppUsers> _userManager = userManager;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<AppUsers> _logger = logger;
        
        #region For Students and Teachers
        public async Task<GenericResponseDto> AddStudent(CreateStudentDto create)
        {
            try
            {
                // 1) Pre-checks
                var existingByEmail =await  _userManager.FindByEmailAsync(create.Email);
                if (existingByEmail != null)
                    throw new BadRequestException($"Email :{create.Email} is Already Used try Another Email");
                if (create.Password != create.ConfirmPassword)
                    throw new BadRequestException("Password and Confirm Password doesn't match.");
                if (!Regex.IsMatch(create.PhoneNumber, @"^(010|011|012|015)[0-9]{8}$"))
                    throw new BadRequestException($"Phone Number :{create.PhoneNumber} is Invalid");
                 if (!Regex.IsMatch(create.ParentContact!, @"^(010|011|012|015)[0-9]{8}$"))
                    throw new BadRequestException($"Phone Number :{create.ParentContact} is Invalid");
                //valid On GradeID if provided
                if (create.GradeID.HasValue)
                {
                    var grade = await _unitOfWork.GetRepository<Grade,int>().GetByIdAsync(create.GradeID.Value);
                    if (grade is  null)
                        throw new NotFoundException($"Grade with ID {create.GradeID.Value} not found.");
                }
                // 2) Prepare profile image
                var profilePath = "images/Default_Icone.png";
                if (create.ProfileImage != null && create.ProfileImage.Length > 0)
                    profilePath = await UploadImageAsync(create.ProfileImage);
                // 3) Create Student (not AppUsers)
                var user = _mapper.Map<Students>(create);
                user.ProfileImage = profilePath;
                user.Gender = create.gender.ToString();
                user.Status = create.Status.ToString();
                // 4) Save Student using UserManager
                var createRes = await _userManager.CreateAsync(user, create.Password);
                if (createRes.Succeeded) {
                    var res = await _userManager.AddToRoleAsync(user, "Student");
                    if (!res.Succeeded)
                    {
                        var errors = res.Errors.Select(e => e.Description);
                        throw new ValidationErrorsExecption(errors);
                    }
                }
                else
                {
                    var errors = createRes.Errors.Select(e => e.Description);
                    throw new ValidationErrorsExecption(errors);
                }
                return new GenericResponseDto() { Message = "Student Created Successfully", IsSuccess = true };


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while Add New Student ");
                return new GenericResponseDto() { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<GenericResponseDto> AddTeacher(CreateTeacherDto create)
        {
            try
            {// 1) Pre-checks
                var existingByEmail = await _userManager.FindByEmailAsync(create.Email);
                if (existingByEmail != null)
                    throw new BadRequestException($"Email :{create.Email} is Already Used try Another Email");

                if (create.Password != create.ConfirmPassword)
                    throw new BadRequestException("Password and Confirm Password doesn't match.");

                if (!Regex.IsMatch(create.PhoneNumber, @"^(010|011|012|015)[0-9]{8}$"))
                    throw new BadRequestException($"Phone Number :{create.PhoneNumber} is Invalid");

                // 2) Prepare profile image
                var profilePath = "images/Default_Icone.png";
                if (create.ProfileImage != null && create.ProfileImage.Length > 0)
                    profilePath = await UploadImageAsync(create.ProfileImage);

                // 3) Create Teacher (not AppUsers)
                var user = mapper.Map<Teacher>(create);
                user.ProfileImage = profilePath;
                user.Gender = create.gender.ToString();
                user.Status = create.Status.ToString();

                var createRes = await _userManager.CreateAsync(user, create.Password);
                if (!createRes.Succeeded)
                {
                    var errors = createRes.Errors.Select(e => e.Description);
                    throw new ValidationErrorsExecption(errors);
                }

               var res= await _userManager.AddToRoleAsync(user, "Teacher");

                if (!res.Succeeded)
                {
                    var errors = res.Errors.Select(e => e.Description);
                    throw new ValidationErrorsExecption(errors);
                }

                return new GenericResponseDto() { Message = "Teacher Created Successfully", IsSuccess = true };
                

            }
            catch(Exception ex) {
                _logger.LogError(ex,"Error while Add New Teacher ");
                return new GenericResponseDto() { Message=ex.Message,IsSuccess= false };
            }
        }
     
        public async Task<GenericResponseDto> DeleteUser(string UserId)
        {
            try
            {
                //check On User Exist
                var user = await _userManager.FindByIdAsync(UserId);
                if (user == null)
                    throw new NotFoundException($"User with ID {UserId} not found.");
                var deleteRes = await _userManager.DeleteAsync(user);
                if (!deleteRes.Succeeded)
                {
                    var errors = deleteRes.Errors.Select(e => e.Description);
                    throw new ValidationErrorsExecption(errors);
                }
              return new GenericResponseDto() { Message = "User Deleted Successfully", IsSuccess = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while Deleting User ");
                return new GenericResponseDto() { IsSuccess=false, Message=ex.Message };
            }
        }
       
        public async Task<GenericResponseDto> UpdateStudent(UpdateStudentDto Update ,string UserId)
        {
            try
            {
                //check On User Exist
                if(string.IsNullOrEmpty(UserId) || Update is null)
                    throw new BadRequestException("UserId Or Update Data cannot be null or empty.");
                var user = await _unitOfWork.GetRepository<Students, string>().GetByIdAsync(UserId);
                if (user == null)
                    throw new NotFoundException($"User with ID {UserId} not found.");
                //check On User Role AS Stuedent
                var isStudent = await _userManager.IsInRoleAsync(user, "Student");
                if (!isStudent)
                    throw new BadRequestException($"User with ID {UserId} is not a Student.");
                //check On Email Change
                #region UpadteUser Data
                if (!string.Equals(user.Email, Update.Email, StringComparison.OrdinalIgnoreCase))
                {
                    var existingByEmail = await _userManager.FindByEmailAsync(Update.Email!);
                    if (existingByEmail != null && existingByEmail.Id != UserId)
                        throw new BadRequestException($"Email :{Update.Email} is Already Used try Another Email");
                    user.Email = Update.Email;
                    user.UserName = Update.Email; // Assuming UserName is same as Email
                }
                //check On Phone Number Change
                if (!string.Equals(user.PhoneNumber, Update.PhoneNumber, StringComparison.OrdinalIgnoreCase))
                {
                    if (!Regex.IsMatch(Update.PhoneNumber!, @"^(010|011|012|015)[0-9]{8}$"))
                        throw new BadRequestException($"Phone Number :{Update.PhoneNumber} is Invalid");
                    user.PhoneNumber = Update.PhoneNumber;
                }
                //check On Parent Contact Change
                if (!string.Equals(user.ParentContact, Update.ParentContact, StringComparison.OrdinalIgnoreCase))
                {
                    if (!Regex.IsMatch(Update.PhoneNumber!, @"^(010|011|012|015)[0-9]{8}$"))
                        throw new BadRequestException($"Phone Number :{Update.PhoneNumber} is Invalid");
                    user.PhoneNumber = Update.PhoneNumber;
                }
                //Check On Profile Image Change
                if (Update.ProfileImage != null && Update.ProfileImage.Length > 0)
                {
                    var profilePath = await UploadImageAsync(Update.ProfileImage);
                    user.ProfileImage = profilePath;
                }
                //check On GradeID Change
                if (Update.GradeID.HasValue && Update.GradeID != user.GradeID)
                {
                    var grade = await _unitOfWork.GetRepository<Grade, int>().GetByIdAsync(Update.GradeID.Value);
                    if (grade is null)
                        throw new NotFoundException($"Grade with ID {Update.GradeID.Value} not found.");
                    user.GradeID = Update.GradeID;
                }
                user.FirstName = Update.FirstName ?? user.FirstName;
                user.LastName = Update.LastName ?? user.LastName;
                user.Address = Update.Address ?? user.Address;
                user.Gender = Update.gender.ToString() ?? user.Gender;
                user.DateOfBirth = Update.DateOfBirth ?? user.DateOfBirth;
                user.ParentName = Update.ParentName ?? user.ParentName;
                user.AssignToSchool = Update.AssignToSchool ?? user.AssignToSchool;
                user.Status = Update.Status.ToString() ?? user.Status;
                user.UpdatedAt = DateTime.UtcNow;

                #endregion
                var res= await  _userManager.UpdateAsync(user);

                if (!res.Succeeded)
                {
                    var errors = res.Errors.Select(e => e.Description);
                    throw new ValidationErrorsExecption(errors);
                }
               return new GenericResponseDto() { Message = "Student Updated Successfully", IsSuccess = true };


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while Updating Student ");
               return new GenericResponseDto() { IsSuccess = false  ,Message=ex.Message};
            }
        }

        public async Task<GenericResponseDto> UpdateTeacher(UpdateTeacherDto update,string UserId)
        {
            try
            {
                //check On User Exist
                if (string.IsNullOrEmpty(UserId) || update is null)
                    throw new BadRequestException("UserId Or Update Data cannot be null or empty.");
                var user =await  _unitOfWork.GetRepository<Teacher, string>().GetByIdAsync(UserId);
                if (user == null)
                    throw new NotFoundException($"User with ID {UserId} not found.");
                //check On User Role AS Teacher
                var isTeacher = await _userManager.IsInRoleAsync(user, "Teacher");
                if (!isTeacher)
                    throw new BadRequestException($"User with ID {UserId} is not a Teacher.");
                //check On Email Change
                #region UpadteUser Data
                if (!string.Equals(user.Email, update.Email, StringComparison.OrdinalIgnoreCase))
                {
                    var existingByEmail = await _userManager.FindByEmailAsync(update.Email!);
                    if (existingByEmail != null && existingByEmail.Id != UserId)
                        throw new BadRequestException($"Email :{update.Email} is Already Used try Another Email");
                    user.Email = update.Email;
                    user.UserName = update.Email; // Assuming UserName is same as Email
                }
                //check On Phone Number Change
                if (!string.Equals(user.PhoneNumber, update.PhoneNumber, StringComparison.OrdinalIgnoreCase))
                {
                    if (!Regex.IsMatch(update.PhoneNumber!, @"^(010|011|012|015)[0-9]{8}$"))
                        throw new BadRequestException($"Phone Number :{update.PhoneNumber} is Invalid");
                    user.PhoneNumber = update.PhoneNumber;
                }
                //Check On Profile Image Change
                if (update.ProfileImage != null && update.ProfileImage.Length > 0)
                {
                    var profilePath = await UploadImageAsync(update.ProfileImage);
                    user.ProfileImage = profilePath;
                }
                user.FirstName = update.FirstName ?? user.FirstName;
                user.LastName = update.LastName ?? user.LastName;
                user.Address = update.Address ?? user.Address;
                user.DateOfBirth = update.DateOfBirth ?? user.DateOfBirth;
                user.HiringDate = update.HiringDate ?? user.HiringDate;
                user.Specialization = update.Specialization ?? user.Specialization;
                user.Status = update.Status.ToString() ?? user.Status;
                user.Gender =update.Gender.ToString()?? user.Gender;
                user.UpdatedAt = DateTime.UtcNow;

                #endregion
                var res = await _userManager.UpdateAsync(user);
                if (res != null) {
                    if (!res.Succeeded)
                    {
                        var errors = res.Errors.Select(e => e.Description);
                        throw new ValidationErrorsExecption(errors);
                    }
                }
                return new GenericResponseDto() { Message = "Teacher Updated Successfully", IsSuccess = true };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while Updating Teacher ");
                return new GenericResponseDto() { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<GenericResponseDto> AssignStudentToClass(string StudentId, string ClassCode)
        {
            try
            {
                //check On User Exist
                if(string.IsNullOrEmpty(StudentId) || string.IsNullOrEmpty(ClassCode))
                    throw new BadRequestException("StudentId Or ClassCode cannot be null or empty.");
                
                var user= await _unitOfWork.GetRepository<Students, string>().GetByIdAsync(StudentId);
                if (user == null)  throw new NotFoundException($"User with ID {StudentId} not found.");

                //check On User Role AS Stuedent
                var isStudent = await _userManager.IsInRoleAsync(user, "Student");
                if (!isStudent)
                    throw new BadRequestException($"User with ID {StudentId} is not a Student.");
                //check On Class Exist
                var classEntity = await _unitOfWork.GetRepository<ClassEntity, string>().GetEntityWithCode<ClassEntity>(ClassCode);
                if(classEntity is null) throw new NotFoundException($"Class with Code {ClassCode} not found.");
                //check On Student Already Assigned In Class
                user.ClassID =classEntity.ClassID;
                var res = await _userManager.UpdateAsync(user);
                if (!res.Succeeded)
                {
                    var errors = res.Errors.Select(e => e.Description);
                    throw new ValidationErrorsExecption(errors);
                }
                return new GenericResponseDto() { Message = "Student Assigned To Class Successfully", IsSuccess = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while Assign Student To Class ");
                return new GenericResponseDto() { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<GenericResponseDto> AssingStudentToSubject(string StudentId, string SubjectCode)
        {
            try
            {
                //Validate Input
                if (string.IsNullOrEmpty(StudentId) || string.IsNullOrEmpty(SubjectCode))
                    throw new BadRequestException("StudentId Or SubjectCode cannot be null or empty.");
                //Check On Student Exist
                var student = await _unitOfWork.GetRepository<Students, string>().GetByIdAsync(StudentId);
                if (student == null)
                    throw new NotFoundException($"Student with ID {StudentId} not found.");
                //Check On User Role AS Stuedent
                var isStudent = await _userManager.IsInRoleAsync(student, "Student");
                if (!isStudent)
                    throw new BadRequestException($"User with ID {StudentId} is not a Student.");
                //Check On Subject Exist
                var subject = await _unitOfWork.GetRepository<Subject, int>().GetEntityWithCode<Subject>(SubjectCode);
                if (subject is null)
                    throw new NotFoundException($"Subject with Code {SubjectCode} not found.");
                //Check On Student Already Assigned In Subject
                var assignment = await _unitOfWork.GetRepository<StudentAssignInSubject, (string, int)>()
                    .GetByIdAsync((StudentId, subject.SubjectID));
                if (assignment != null)
                    throw new BadRequestException($"Student with ID {StudentId} is already assigned to Subject {SubjectCode}.");
                //Create New Assignment
                var studentAssignInSubject = new StudentAssignInSubject
                {
                    StudentId = StudentId,
                    SubjectId = subject.SubjectID,
                    SubjectName = subject.SubjectName,
                    UserName = student.UserName!
                };
                //Assign Student to Subject
                _unitOfWork.GetRepository<StudentAssignInSubject, (string, int)>().AddAsync(studentAssignInSubject);

                var res= await _unitOfWork.SaveChanges();
                
                var Response = new GenericResponseDto();
                if (res <= 0)
                    throw new BadRequestException("Failed to assign Student to Subject.");
               
                    Response.IsSuccess = true;
                    Response.Message = "Student assigned to Subject successfully.";
                
                return Response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while Assign Student To Subject ");
                return new GenericResponseDto() { IsSuccess = false ,Message =ex.Message };
            }
        }

        public async Task<GenericResponseDto> AssignTeacherToClass(string TeacherId, string ClassCode)
        {
            try
            {
                //Validate Input
                if (string.IsNullOrEmpty(TeacherId) || string.IsNullOrEmpty(ClassCode))
                    throw new BadRequestException("TeacherId Or ClassCode cannot be null or empty.");
                //Check On Teacher Exist
                var teacher = await _unitOfWork.GetRepository<Teacher, string>().GetByIdAsync(TeacherId);
                if (teacher == null)
                    throw new NotFoundException($"Teacher with ID {TeacherId} not found.");
                //Check On User Role AS Teacher
                var isTeacher = await _userManager.IsInRoleAsync(teacher, "Teacher");
                if (!isTeacher)
                    throw new BadRequestException($"User with ID {TeacherId} is not a Teacher.");
                //Check On Class Exist
                var classEntity = await _unitOfWork.GetRepository<ClassEntity, string>().GetEntityWithCode<ClassEntity>(ClassCode);
                if(classEntity is null) throw new NotFoundException("Class with Code {ClassCode} not found.");

                //Check On Teacher Already Assigned In Class
                 var TeacherClassAssignment = await _unitOfWork.GetRepository<TeacherClass, (string, int)>()
                    .GetByIdAsync((TeacherId, classEntity.ClassID));
                if (TeacherClassAssignment != null)
                    throw new BadRequestException($"Teacher with ID {TeacherId} is already assigned to Class {ClassCode}.");
                //Create New Assignment
                var teacherClass = new TeacherClass
                {
                    TeacherId = TeacherId,
                    ClassID = classEntity.ClassID
                };
                //Assign Teacher to Class
                _unitOfWork.GetRepository<TeacherClass, (string, int)>().AddAsync(teacherClass);
               
                var res = await _unitOfWork.SaveChanges();

                var response = new GenericResponseDto();
                if (res <= 0)
                throw new BadRequestException("Failed to assign Teacher to Class.");
               
                    response.IsSuccess = true;
                    response.Message = "Teacher assigned to Class successfully.";
                
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while Assign Teacher To Class ");
                return new GenericResponseDto()
                { IsSuccess = false,
                    Message = "Failed to assign Teacher to Class." };

            }
        }

        public async  Task<GenericResponseDto> AssingTeacherToSubject(string TeacherId, string SubjectCode)
        {
            try
            {
                //validate Input
                if (string.IsNullOrEmpty(TeacherId) || string.IsNullOrEmpty(SubjectCode))
                    throw new BadRequestException("TeacherId Or SubjectCode cannot be null or empty.");
                //Check On Teacher Exist
                var teacher = await _unitOfWork.GetRepository<Teacher, string>().GetByIdAsync(TeacherId);
                if (teacher == null)
                    throw new NotFoundException($"Teacher with ID {TeacherId} not found.");
                //Check On User Role AS Teacher
                var isTeacher = await _userManager.IsInRoleAsync(teacher, "Teacher");
                if (!isTeacher)
                    throw new BadRequestException($"User with ID {TeacherId} is not a Teacher.");
                //Check On Subject Exist
                var subject = await _unitOfWork.GetRepository<Subject, int>().GetEntityWithCode<Subject>(SubjectCode);
                if (subject is null)
                    throw new NotFoundException($"Subject with Code {SubjectCode} not found.");
                //Check On Teacher Already Assigned In Subject
                if(teacher.Subjects.Contains(subject))
                    throw new BadRequestException("$Teacher with ID {TeacherId} is already assigned to Subject {SubjectCode}.");
                //Create New Assignment
                teacher.Subjects.Add(subject);
                var res = await _userManager.UpdateAsync(teacher);
                if (!res.Succeeded)
                {
                    var errors = res.Errors.Select(e => e.Description);
                    throw new ValidationErrorsExecption(errors);
                }
                return new GenericResponseDto()
                {
                    IsSuccess = true,
                    Message = "Teacher assigned to Subject successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while Assign Teacher To Subject ");
                return new GenericResponseDto()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<GenericResponseDto> RemoveStudentFromClass(string StudentId, string ClassCode)
        {
            try
            {
                //validate Input
                if (string.IsNullOrEmpty(StudentId) || string.IsNullOrEmpty(ClassCode))
                    throw new BadRequestException("StudentId Or ClassCode cannot be null or empty.");
                //Check On Student Exist
                var AssignStudentInClass = await _unitOfWork.GetRepository<Students, string>().GetByIdAsync(StudentId);
                if (AssignStudentInClass == null)
                    throw new NotFoundException($"Student with ID {StudentId} not found.");
                //Check On User Role AS Stuedent
                var isStudent = await _userManager.IsInRoleAsync(AssignStudentInClass, "Student");
                if (!isStudent)
                    throw new BadRequestException($"User with ID {StudentId} is not a Student.");
                //Check On Class Exist
                var classEntity = await _unitOfWork.GetRepository<ClassEntity, string>().GetEntityWithCode<ClassEntity>(ClassCode);
                if (classEntity is null) throw new NotFoundException($"Class with Code {ClassCode} not found.");
                //Check On Student Assigned In Class
                if(AssignStudentInClass.ClassID != classEntity.ClassID)
                    throw new BadRequestException($"Student with ID {StudentId} is not assigned to Class {ClassCode}.");
                //Remove Student From Class
                AssignStudentInClass.ClassID = null;
                var res = await _userManager.UpdateAsync(AssignStudentInClass);
                if (!res.Succeeded)
                {
                    var errors = res.Errors.Select(e => e.Description);
                    throw new ValidationErrorsExecption(errors);
                }
                return new GenericResponseDto()
                {
                    IsSuccess = true,
                    Message = "Student removed from Class successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while Remove Student From Class ");
                return new GenericResponseDto()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };

            }
        }

        public async Task<GenericResponseDto> RemoveStudentFromSubject(string StudentId, string SubjectCode)
        {
            try
            {
                //validate Input
                if (string.IsNullOrEmpty(StudentId) || string.IsNullOrEmpty(SubjectCode))
                    throw new BadRequestException("StudentId Or SubjectCode cannot be null or empty.");
                
                //Check On Subject Exist
                var subject = await _unitOfWork.GetRepository<Subject, int>().GetEntityWithCode<Subject>(SubjectCode);
                if (subject is null)
                    throw new NotFoundException($"Subject with Code {SubjectCode} not found.");
                //Check On Student Assigned In Subject
                var assignment = await _unitOfWork.GetRepository<StudentAssignInSubject, (string, int)>()
                    .GetByIdAsync((StudentId, subject.SubjectID));
                if (assignment == null) throw new NotFoundException($"Student with ID {StudentId} is not assigned to Subject {SubjectCode}.");
                //Remove Student From Subject
                _unitOfWork.GetRepository<StudentAssignInSubject, (string, int)>().DeleteAsync(assignment);
                var res = await _unitOfWork.SaveChanges();
                if(res<=0)
                    throw new BadRequestException("Failed to remove Student from Subject.");
                return new GenericResponseDto()
                {
                    IsSuccess = true,
                    Message = "Student removed from Subject successfully."
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while Remove Student From Subject ");
                return new GenericResponseDto()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
      
        public async Task<GenericResponseDto> RemoveTeacherFromClass(string TeacherId, string ClassCode)
        {
            try
            {
                //validate Input
                if (string.IsNullOrEmpty(TeacherId) || string.IsNullOrEmpty(ClassCode))
                    throw new BadRequestException("TeacherId Or ClassCode cannot be null or empty.");
                //Get Class Id
                var classEntity = await _unitOfWork.GetRepository<ClassEntity, string>().GetEntityWithCode<ClassEntity>(ClassCode);

                //check Teacher Is Assign To calss 
                var teachrerClassAssignment = await _unitOfWork.GetRepository<TeacherClass, (string, int)>().GetByIdAsync((TeacherId, classEntity!.ClassID));

                if (teachrerClassAssignment == null)
                    throw new NotFoundException($"Teacher with ID {TeacherId} is not assigned to Class {ClassCode}.");
                //Remove Teacher From Class
                _unitOfWork.GetRepository<TeacherClass, (string, int)>().DeleteAsync(teachrerClassAssignment);
                var res = await _unitOfWork.SaveChanges();
                if (res <= 0)
                    throw new BadRequestException("Failed to remove Teacher from Class.");
                return new GenericResponseDto()
                {
                    IsSuccess = true,
                    Message = " Teacher removed from Class successfully."
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while Remove Teacher From Class ");
                return new GenericResponseDto()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<GenericResponseDto> RemoveTeacherFromSubject(string TeacherId, string SubjectCode)
        {
            try
            {
                //validate Input
                if (string.IsNullOrEmpty(TeacherId) || string.IsNullOrEmpty(SubjectCode))
                    throw new BadRequestException("TeacherId Or SubjectCode cannot be null or empty.");
                //Check On Teacher Exist
                var teacher = await _unitOfWork.GetRepository<Teacher, string>().GetByIdAsync(TeacherId);
                if (teacher == null)
                    throw new NotFoundException($"Teacher with ID {TeacherId} not found.");
                //Check On User Role AS Teacher
                var isTeacher = await _userManager.IsInRoleAsync(teacher, "Teacher");
                if (!isTeacher)
                    throw new BadRequestException($"User with ID {TeacherId} is not a Teacher.");
                //Check On Subject Exist
                var subject = await _unitOfWork.GetRepository<Subject, int>().GetEntityWithCode<Subject>(SubjectCode);
                if (subject is null)
                    throw new NotFoundException($"Subject with Code {SubjectCode} not found.");
                //Check On Teacher Assigned In Subject
                if (!teacher.Subjects.Contains(subject))
                    throw new NotFoundException($"Teacher with ID {TeacherId} is not assigned to Subject {SubjectCode}.");
                //Remove Teacher From Subject
                teacher.Subjects.Remove(subject);
                var res = await _userManager.UpdateAsync(teacher);
                if (!res.Succeeded)
                {
                    var errors = res.Errors.Select(e => e.Description);
                    throw new ValidationErrorsExecption(errors);
                }
                return new GenericResponseDto()
                {
                    IsSuccess = true,
                    Message = " Teacher removed from Subject successfully."
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while Remove Teacher From Subject ");
                return new GenericResponseDto()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        #endregion





        public Task<string> UpdateClass(string ClassCode, ClassCreateOrUpdate update, string GradeCode)
        {
            throw new Exception("Not Implemented");
        }


        public Task<string> AddClass(string GradeCode, ClassCreateOrUpdate create)
        {
            throw new NotImplementedException();
        }

        public Task<AdminDashboardDto> adminDashboardDto()
        {
            throw new NotImplementedException();
        }

        public Task<ClassDetialsResponseDto> classDetialsResponseDto(string ClassCode)
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteClass(string ClassCode)
        {
            throw new NotImplementedException();
        }

      

        public Task<AdminProfileDto> getAdminProfile(string UserId)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResponse<ClassShortResponseDto>> GetAllClasses(ClassFilteration filter)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResponse<StudentShortResponseDto>> GetAllStudentAssingedInClass(string ClassCode, USerFilteration filter)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResponse<StudentShortResponseDto>> GetAllStudentAssingedInSubject(string SubjectCode, USerFilteration filter)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResponse<StudentShortResponseDto>> GetAllStudents(USerFilteration filter)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResponse<StudentShortResponseDto>> GetAllStudentsAssingInGrade(string GradeCode, USerFilteration filter)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResponse<SubjectResponseShortDto>> GetAllSubjectTecherAssignIn(string TeacherId)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResponse<TeacherShortResponseDto>> GetAllTeachers(USerFilteration filter)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResponse<TeacherShortResponseDto>> GetAllTeachersAssingInClass(string ClassCode, USerFilteration filter)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResponse<TeacherShortResponseDto>> GetAllTeachersAssingInGrade(string GradeCode, USerFilteration filter)
        {
            throw new NotImplementedException();
        }

        public Task<StudentResponseDetailsDto> GetStudentDetailsByCode(string StudentCode)
        {
            throw new NotImplementedException();
        }

        public Task<TeacherResultDto> GetTeaherDetailsByCode(string TeacherCode)
        {
            throw new NotImplementedException();
        }

        public Task<string> updateAdminProfile(string UserId, UpdateAdminProfileDto adminProfileDto)
        {
            throw new NotImplementedException();
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
