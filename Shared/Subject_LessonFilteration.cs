using Shared.IdentityDtos;
using Shared.SubjectDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class Subject_LessonFilteration
    {
        public SortingSubjects? Sorting { get; set; }
        public string? SearchKey { get; set; }

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
