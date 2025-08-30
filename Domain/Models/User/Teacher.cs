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
        public IList<int>? AssignedSubjects { get; set; } = new List<int>(); //
        public IList<int>? AssignedClasses { get; set; } = new List<int>(); //
        public string Status { get; set; } = "Active"; // Active, Inactive, Resigned
    }
}
