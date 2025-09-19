using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public  class PaginationResponse<T> where T : class
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public int Total { get; set; }
        public IEnumerable<T> Data { get; set; } = [];
    }
}
