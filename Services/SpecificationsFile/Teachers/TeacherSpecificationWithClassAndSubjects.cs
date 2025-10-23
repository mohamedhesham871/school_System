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
    public class TeacherSpecificationWithClassAndSubjects : Specifications<Teacher>
    {

        //Get By   Id With Class And Subjects
        public TeacherSpecificationWithClassAndSubjects(string TeacherId) : base(t => t.Id == TeacherId)
        {
            AddInclude(t => t.TeacherClasses!);
            AddInclude(t => t.Subjects!);
        }

        public TeacherSpecificationWithClassAndSubjects(USerFilteration teacherFilteration)
        {
            AddInclude(t => t.Subjects!);
            AddInclude(t => t.TeacherClasses!);
            //sorting 
            Sorting(teacherFilteration);

            if (teacherFilteration.PageIndex > 0)
            {
                ApplyPaging(teacherFilteration.PageIndex,15);
            }
        }
        //For Searching By name Or UserName Or Filtering By SubjectId Or ClassId and Counting

        private void Sorting(USerFilteration teacherFilteration)
        {
            switch (teacherFilteration.Sorting)
            {
                case SortingExpression.NameAsc:
                    AddOrderBy(t => t.FirstName);
                    break;
                case SortingExpression.NameDesc:
                    AddOrderByDescending(t => t.FirstName);
                    break;
                case SortingExpression.HiringDateAsc:
                    AddOrderBy(t => t.HiringDate);
                    break;
                case SortingExpression.HiringDateDesc:
                    AddOrderByDescending(t => t.HiringDate);
                    break;
                default:
                    AddOrderBy(t => t.UserName);
                    break;
            }
        }
    }
}
