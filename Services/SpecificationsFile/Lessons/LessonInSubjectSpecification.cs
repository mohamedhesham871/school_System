using Domain.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shared.SubjectDtos;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.SpecificationsFile.Lessons
{
    public class LessonInSubjectSpecification : Specifications<Lesson>
    {
        public LessonInSubjectSpecification()
        {

        }
        public LessonInSubjectSpecification(string code) : base(l => l.Code == code)
        {
            AddInclude(l => l.Subject!);
            AddInclude(l => l.quiz!);
        }
        public LessonInSubjectSpecification(Subject_LessonFilteration subjectFilteration, int subjectId, bool OnlyActive = false)
            : base(l => l.SubjectId == subjectId && (OnlyActive ? l.IsActive : true))
        {

            AddInclude(t => t.Subject!);
            AddInclude(t => t.quiz!);
            //sorting 
            Sorting(subjectFilteration);

            if (subjectFilteration.PageSize > 0 && subjectFilteration.PageIndex > 0)
            {
                ApplyPaging(subjectFilteration.PageIndex, subjectFilteration.PageSize);
            }
        }
        //For Teacher to have Access to all Lesson Active or Not 


        private void Sorting(Subject_LessonFilteration LessonFilteration)
        {
            switch (LessonFilteration.Sorting)
            {
                case SortingSubjects.SubjectNameAsc:
                    AddOrderBy(t => t.Title);
                    break;
                case SortingSubjects.SubjectNameDesc:
                    AddOrderByDescending(t => t.Title);
                    break;
                case SortingSubjects.CreatedAtAsc:
                    AddOrderBy(t => t.CreatedAt);
                    break;
                case SortingSubjects.CreatedAtDesc:
                    AddOrderByDescending(t => t.CreatedAt);
                    break;
                default:
                    AddOrderBy(t => t.Order);
                    break;
            }
        }
    }
}
