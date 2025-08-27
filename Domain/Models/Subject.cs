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
        public string SubjectName { get; set; } = string.Empty;
        public int GradeID { get; set; } // Foreign key to Grades table
        public int TeacherID { get; set; } // Foreign key to Teachers table
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}

