using Microsoft.EntityFrameworkCore.ChangeTracking;
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
        public int? GradeId;

        public SortingClass? Sorting { get; set; }
        public string? SearchKey { get; set; }

        public int PageIndex { get; set; } = 1;

        private int pageSize { get; set; } 
        private const int MaxPageSize = 20;
        private const int DefaultPageSize = 10;
        public  int PageSize
        {
            get => pageSize;
            set => pageSize = (value > MaxPageSize|| value< DefaultPageSize) ? DefaultPageSize : value;
        }
    }
}
