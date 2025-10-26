using Shared;
using Shared.IdentityDtos;
using Shared.IdentityDtos.Admin;
using Shared.SubjectDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractionServices
{
    public  interface IAdminServices
    {
        //For Student
        Task<GenericResponseDto> AddStudent(CreateStudentDto create);
        Task<GenericResponseDto> UpdateStudent(UpdateStudentDto Update,string UserId);
        Task<GenericResponseDto> AssignStudentToClass(string StudentId, string ClassCode);
        Task<GenericResponseDto> AssingStudentToSubject(string StudentId, string SubjectCode);
        Task<GenericResponseDto> RemoveStudentFromClass(string StudentId, string ClassCode);
        Task<GenericResponseDto> RemoveStudentFromSubject(string StudentId, string SubjectCode);
        //For Teacher
        Task<GenericResponseDto> AddTeacher(CreateTeacherDto create);
        Task<GenericResponseDto> UpdateTeacher(UpdateTeacherDto update,string UserId);
        Task<GenericResponseDto> AssignTeacherToClass(string TeacherId, string ClassCode);
        Task<GenericResponseDto> AssingTeacherToSubject(string TeacherId, string SubjectCode);
        Task<GenericResponseDto> RemoveTeacherFromClass(string TeacherId, string ClassCode);
        Task<GenericResponseDto> RemoveTeacherFromSubject(string TeacherId, string SubjectCode);
        //For Any User
        Task<GenericResponseDto> DeleteUser(string UserId);

                             //Done

        //Other Admin Related Services
        Task<AdminDashboardDto> adminDashboardDto();
        Task<AdminProfileDto> getAdminProfile(string UserId);
        Task<GenericResponseDto> updateAdminProfile(string UserId, UpdateAdminProfileDto adminProfileDto);
       
        #region For Student Management
        Task<PaginationResponse<StudentShortResponseDto>> GetAllStudents(USerFilteration filter);
        Task<StudentResponseDetailsDto> GetStudentDetailsByCode(string StudentCode);
        //For Grade, Class, Subject Assignment
        //1 - GetAllStudentsAssingInGrade 
        //2-  GetAllStudentAssingedInClass
        //3- GetAllStudentAssingedInSubject

        #endregion

        #region For Teacher Management
        Task<PaginationResponse<TeacherShortResponseDto>> GetAllTeachers(USerFilteration filter);
        //For Grade, Class Assignment
        // GetAllTeachersAssingInGrade
        // GetAllTeachersAssingInClass
        // GetAllSubjectTecherAssignIn
        Task<TeacherDetailsForAdminDto> GetTeaherDetailsByCode(string TeacherCode);

        #endregion

        //For Class Management
        #region ForClassManagement
     
        #endregion

    }
}