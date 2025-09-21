using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.SubjectDtos
{
    public class SubjectResponseShortDto
    {
        public string SubjectName { get; set; } = null!;
        public string SubjectCode { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        
    }
}
