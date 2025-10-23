using Domain.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.SpecificationsFile.Lessons
{
    public class LessonSpecCount : Specifications<Lesson>
    {
        public LessonSpecCount(Subject_LessonFilteration f, int id, bool OnlyActive = false)
            : base(Criteria(f, id, OnlyActive))
        {
        }
        public LessonSpecCount()
        {

        }

        private static Expression<Func<Lesson, bool>> Criteria(Subject_LessonFilteration f, int id, bool OnlyActive = false)
        {
            var search = f.SearchKey?.Trim().ToLower();

            return s =>
                s.SubjectId == id &&
                (!OnlyActive || s.IsActive) &&
                (string.IsNullOrEmpty(search) ||
                 s.Title != null && s.Title.ToLower().Contains(search));
        }
    }
}
