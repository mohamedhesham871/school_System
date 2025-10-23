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
        public int? GradeId { get; set; }
        public int? SubjectId { get; set; }
        public int? ClassId { get; set; }
       
        public SortingExpression? Sorting { get; set; }
        public string? SearchKey { get; set; }
        public UserState? State { get; set; }    

        public int PageIndex { get; set; } = 1;
        private const int PageSize = 15;
    }
}
