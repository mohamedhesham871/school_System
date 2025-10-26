using Domain.Models.User;
using Shared;
using Shared.IdentityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.SpecificationsFile.StudentSpec
{
    public  class StudentSpecification:Specifications<Students>
    {
        public StudentSpecification(string userId):base(s=>s.Id==userId)
        {
            AddInclude(s => s.Grade);
            AddInclude(s => s.Class);
            AddInclude(s => s.StudentAssignInSubjects!);
        }

        public StudentSpecification(USerFilteration f,bool CountOnly) : base(s =>
                     (string.IsNullOrEmpty(f.GradeCode) || s.Grade.GradeCode == f.GradeCode) &&
                     (string.IsNullOrEmpty(f.ClassCode) || s.Class.Code == f.ClassCode) &&
                     (!f.State.HasValue|| s.Status==f.State.Value.ToString()) &&
                     (string.IsNullOrEmpty(f.SearchKey) || s.UserName.Contains(f.SearchKey)))
        {
            if (!CountOnly)
            {
                AddInclude(s => s.Grade);
                AddInclude(s => s.Class);

                Sorting(f);

                if (f.PageIndex <= 0) f.PageIndex = 1;
                ApplyPaging(f.PageIndex, 15);
            }
        }


        private void Sorting(USerFilteration f)
        {
            switch (f.Sorting)
            {
                case SortingExpression.NameDesc:
                    AddOrderByDescending(s => s.UserName);
                    break;
                case SortingExpression.NameAsc:
                    AddOrderBy(s => s.UserName);
                    break;
                case SortingExpression.HiringDateDesc:
                    AddOrderBy(s => s.AssignToSchool); break;
                case SortingExpression.HiringDateAsc:
                    AddOrderByDescending(s => s.AssignToSchool);
                    break;
                default:
                    AddOrderBy(s => s.UserName);
                    break;
            }
        }
    }
}
