using Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    
    public class TeacherClass
    {
        public string TeacherId { get; set; } 
        public Teacher Teacher { get; set; }
       
        public int ClassID { get; set; }
        public ClassEntity Class { get; set; }
    }
}
