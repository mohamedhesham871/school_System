using Shared.GradeDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ClassDetialsResponseDto
    {
        public string ClassCode { get; set; }
        public string ClassName { get; set; }
        public GradeResponseShortDto GradeResponseShortDto { get; set; }
    }
}
