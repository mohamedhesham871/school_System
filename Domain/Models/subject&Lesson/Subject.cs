using Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public  class Subject
    {
        public int SubjectID { get; set; } 
        public string SubjectCode { get; set; } = string.Empty; // Unique code for the subject (e.g., "MATH-101")
        public string SubjectName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        #region Relation Teacher with Subject
        public Teacher? Teacher { get; set; } // Navigation property to Teacher
        public string? TeacherId { get; set; } // Foreign key to Teacher 
        #endregion

        #region Relation Subject and Grade M:1 
        //Eacher Subject belongs to one Grade and Grade can have multiple Subjects
        public int GradeID { get; set; } // Foreign key to Grades table
        public Grade Grade { get; set; } = null!; // Navigation property to Grade
        #endregion
        //there is Relation with Lesson 1:M so FK in Lesson Table
        public ICollection<Lesson?> Lessons { get; set; } = [];
    }
}

