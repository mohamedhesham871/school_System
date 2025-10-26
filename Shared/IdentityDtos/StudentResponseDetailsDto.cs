using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.IdentityDtos
{
    public  class StudentResponseDetailsDto
    {
        public string UserId { get; set; }
        public string FirstName { get;set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string parentNumber { get; set; }
        public string UserName { get; set; }

        public string ClassName {  get; set; }
        public string GradeName { get;set;}
        public ICollection<string> SubjectName {  get; set; } 
    }
}
