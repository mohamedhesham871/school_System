using Domain.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.SpecificationsFile.Classes
{
    public  class ClassSpecificationCount:Specifications<ClassEntity> 
    {
        public ClassSpecificationCount(ClassFilteration f):base(Criteria(f))
        {
           
        }

        private static Expression<Func<ClassEntity, bool>> Criteria(ClassFilteration f)
        {
            return c => (string.IsNullOrEmpty(f.GradeCode) ||
         c.Grade.GradeCode == f.GradeCode) &&
        (string.IsNullOrEmpty(f.SearchKey) ||
         c.ClassName.Contains(f.SearchKey)); 
        }

    }
}
