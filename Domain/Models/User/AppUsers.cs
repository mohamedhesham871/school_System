using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.User
{
    public class AppUsers : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? ProfileImage { get; set; } = "images/Default.png";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
/*
 Student
Field	Type	Description
StudentID	GUID/INT	Unique student ID   ==found 
FirstName	String	First name              ==Found
LastName	String	Last name              ==Found
DateOfBirth	Date	Birth date             ==Found
Gender	Enum(Male, Female)	Gender         ==Found
Address	String	Home address               ==Found
ParentName	String	Guardian name          == Not Found
ParentContact	String	Guardian contact    == Not Found
Email	String	Login/communication       == Found
GradeID	FK	Current grade                == Not Found 
ClassID	FK	Assigned class               == Not Found
EnrollmentDate	Date	Enrollment date    ==Found
Status	Enum(Active, Inactive, Graduated)	Current status == Not Found
ProfileImage	String (URL)	Profile picture
CreatedAt	DateTime	Record created date
UpdatedAt	DateTime	Last modified date
*/