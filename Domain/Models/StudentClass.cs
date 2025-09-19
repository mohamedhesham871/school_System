using Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public  class StudentClass
    {
        public int ClassID { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public Grade Grade { get; set; } //  nav to Grade
        public int GradeID { get; set; } // Foreign key to Grades table
        public ICollection<TeacherClass?> TeacherClasses { get; set; } = [];
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime UpdateAt { get; set; } =DateTime.Now;


        //  public ICollection<Teacher>? Teachers { get; set; } = [];//nav prop

    }
}
