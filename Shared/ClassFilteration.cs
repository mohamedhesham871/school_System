using Shared.IdentityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ClassFilteration
    {
        public string? GradeCode;

        public SortingClass? Sorting { get; set; }
        public string? SearchKey { get; set; }

        public int PageIndex { get; set; } = 1;
        public int PageSize { get; } = 10;
    }
}
