using Domain.Models.User;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public  class ClassEntity:IHasCode
    {
        public int ClassID { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public Grade Grade { get; set; } //  nav to Grade
        public int GradeID { get; set; } // Foreign key to Grades table
        public ICollection<TeacherClass?> TeacherClasses { get; set; } = [];
        public ICollection<Students>? Students { get; set; } = [];//nav prop
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime UpdateAt { get; set; } =DateTime.Now;
        public string Code { get; set; }

        public ClassEntity()
        {
            Code= Guid.NewGuid().ToString();
        }


        //  public ICollection<Teacher>? Teachers { get; set; } = [];//nav prop

    }
}
