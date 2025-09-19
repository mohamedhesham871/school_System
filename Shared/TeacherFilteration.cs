using Shared.IdentityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class TeacherFilteration
    {
        public int? SubjectId { get; set; }
        public int? ClassId { get; set; }
       
        public SortingExpression? Sorting { get; set; }
        public string? SearchKey { get; set; }
        public TeacherState? State { get; set; }    

        public int PageIndex { get; set; } = 1;
        private int pageSize;
        private const int PageMaxSize = 15;
        private const int DefaultPageSize = 5;
        
        public int PageSize
        {
            get { return pageSize; }
            set
            {
                if (value > 0 && value <= PageMaxSize)
                    pageSize = value;
                else
                    pageSize = DefaultPageSize;
            }
        }
    }
}
