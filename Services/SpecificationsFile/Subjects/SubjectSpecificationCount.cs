using Domain.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Microsoft.Extensions.Logging;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Subjectt = Domain.Models.Subject;

namespace Services.SpecificationsFile.Subjects
{
    public class SubjectSpecificationCount(Subject_LessonFilteration f) : Specifications<Subjectt>(criteria(f))
    {
        public static Expression<Func<Subjectt, bool>> criteria(Subject_LessonFilteration f)
        {
            var search = f.SearchKey?.Trim().ToLower();

            return s =>
                string.IsNullOrEmpty(search)
                    || s.SubjectName != null && s.SubjectName.ToLower().Contains(search)
                    || s.Code != null && s.Code.ToLower().Contains(search);
        }
    }

}
