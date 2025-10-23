using Domain.Models.User;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
namespace Services.SpecificationsFile.Teachers
{
    public class TeacherCountSpecifications(USerFilteration f) : Specifications<Teacher>(criteria(f))
    {
        public static Expression<Func<Teacher, bool>> criteria(USerFilteration f)
        {
            var search = f.SearchKey?.Trim().ToLower();

            return t =>
                // Search by name/username 
                (string.IsNullOrEmpty(search)
                    || t.FirstName != null && t.FirstName.ToLower().Contains(search)
                    || t.LastName != null && t.LastName.ToLower().Contains(search)
                    || t.UserName != null && t.UserName.ToLower().Contains(search))
                // Optional status filter (adjust to your enum/string mapping)
                && (!f.State.HasValue || t.Status == f.State.Value.ToString())
                // Filter by Subject (Teacher.Subjects is ICollection<Subject>)
                && (!f.SubjectId.HasValue
                    || t.Subjects != null && t.Subjects.Any(s => s.SubjectID == f.SubjectId.Value))
                // Filter by Class (via join entity: Teacher.TeacherClasses)
                && (!f.ClassId.HasValue
                    || t.TeacherClasses != null && t.TeacherClasses.Any(tc => tc.ClassID == f.ClassId.Value));
        }
    }
}
