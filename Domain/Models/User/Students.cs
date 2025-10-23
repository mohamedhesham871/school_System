using Domain.Models.subject_Lesson;
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
        public DateTime AssignToSchool { get; set; }
        public int? GradeID { get; set; } //Nullable foreign key to Grades table Cause the student may not be assigned to a grade yet OR can Be Graduated OR Left the school
        public int? ClassID { get; set; } // Same as GradeID, Nullable foreign key to Classes table
        public string Status { get; set; } = "Active"; // Active, Inactive, Graduated

        //Nav Prop
        public ICollection<QuizStudent>? QuizStudents { get; set;} = new List<QuizStudent>();
        public ICollection<StudentAssignInSubject>? StudentAssignInSubjects { get; set; } = [];
        
        public Grade Grade { get; set; } = null!;
        public ClassEntity Class { get; set; } = null!;

      
    }
}
