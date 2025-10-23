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
        public int ClassID { get; set; } //PK 

        public string Code { get; set; } //Internal PK

        public string ClassName { get; set; } = string.Empty;
        public int GradeID { get; set; } // Fk

        //Navigation Properties 
        public Grade Grade { get; set; } //  nav to Grade
        public ICollection<TeacherClass?> TeacherClasses { get; set; } = [];
        public ICollection<Students>? Students { get; set; } = [];//nav prop

        //Addtional Properties
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime UpdateAt { get; set; } =DateTime.Now;

        //Constructor
        public ClassEntity()
        {
            Code= Guid.NewGuid().ToString();
        }

    }
}
