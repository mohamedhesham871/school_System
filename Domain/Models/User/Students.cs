using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.User
{
    public class Students : AppUsers
    {
        public string? ParentName { get; set; } = null!;
        public string? ParentContact { get; set; } = null!;
        public DateTime EnrollmentDate { get; set; }
        public int? GradeID { get; set; } //Nullable foreign key to Grades table Cause the student may not be assigned to a grade yet OR can Be Graduated OR Left the school
        public int? ClassID { get; set; } // Same as GradeID, Nullable foreign key to Classes table
        public string Status { get; set; } = "Active"; // Active, Inactive, Graduated
    }
}

/*
 Student
Field	Type	Description
StudentID	GUID/INT	Unique student ID   ==found 
FirstName	String	First name              ==Found
LastName	String	Last name               ==Found
DateOfBirth	Date	Birth date              ==Found
Gender	Enum(Male, Female)	Gender          ==Found
Address	String	Home address                ==Found
ParentName	String	Guardian name           == Not Found
ParentContact	String	Guardian contact    == Not Found
Email	String	Login/communication         == Found
GradeID	FK	Current grade                   == Not Found 
ClassID	FK	Assigned class                  == Not Found
EnrollmentDate	Date	Enrollment date     ==Found
Status	Enum(Active, Inactive, Graduated)	Current status == Not Found
ProfileImage	String (URL)	Profile picture
CreatedAt	DateTime	Record created date
UpdatedAt	DateTime	Last modified date
*/