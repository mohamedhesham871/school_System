using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.User
{
    public class Teacher : AppUsers
    {
        public DateTime HiringDate { get; set; }
        public string Specialization { get; set; } = null!;
        public string Status { get; set; } = "Active"; // Active, Inactive, Resigned

        #region Teacher With Subjects
        //nav prop
        public ICollection<Subject?> Subjects { get; set; } = [];//this is Assigned Subject for One Teacher 
        #endregion

        #region Teacher With Classes 
        //nav
        public ICollection<TeacherClass?> TeacherClasses { get; set; } = [];
        #endregion
    }
}
