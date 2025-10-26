using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.IdentityDtos.Admin
{
    public  class AdminDashboardDto
    {
        
        //{ totalStudents, totalTeachers, totalClasses, totalSubjects,  }
        public int totalStudents{ get;set; }
        public int totalStudentsActive { get;set; }
        public int totalTeachers { get;set; }
        public int totalTechersActive { get;set; }
        public int totalClasses { get;set; }
        public int totalSubjects { get;set; }


    }
}
