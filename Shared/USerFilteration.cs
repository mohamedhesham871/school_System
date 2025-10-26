using Shared.IdentityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class USerFilteration
    {
        public string? GradeCode { get; set; }
        public string? SubjectCode { get; set; }
        public string? ClassCode { get; set; }
       
        public SortingExpression? Sorting { get; set; }
        public string? SearchKey { get; set; }
        public UserState? State { get; set; }    

        public int PageIndex { get; set; } = 1;
        private const int PageSize = 15;
    }
}
