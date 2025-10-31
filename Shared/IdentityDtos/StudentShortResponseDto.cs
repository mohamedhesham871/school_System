using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.IdentityDtos
{
    public  class StudentShortResponseDto
    { 
        public string? StudentId { get; set; }
        public string? studentEmail { get; set; }
        public string? userName { get; set; }
        public string? ClassName { get; set; }
        public string? GradeName { get; set; }
        public string? status { get; set; }


    }
}
