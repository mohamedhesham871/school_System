using Domain.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.SpecificationsFile
{
    public class SubjectSpecificationCount(Subject_LessonFilteration f) : Specifications<Subject>(criteria(f))
    {
        public static Expression<Func<Subject, bool>> criteria(Subject_LessonFilteration f)
        {
            var search = f.SearchKey?.Trim().ToLower();

            return s =>
                (string.IsNullOrEmpty(search)
                    || (s.SubjectName != null && s.SubjectName.ToLower().Contains(search))
                    || (s.Code != null && s.Code.ToLower().Contains(search)));
        }
    }

}
