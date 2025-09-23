using Domain.Models;
using Shared;
using Shared.SubjectDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.SpecificationsFile
{
    public class SubjectSpecificationWithGradeAndLessonAndTeacher:Specifications<Subject>
    {
        //get Subject By Id[subjet Code ] With Grade And Lessons And Teacher
        public SubjectSpecificationWithGradeAndLessonAndTeacher(string SubjectCode):base(s=>s.Code==SubjectCode)
        {
            AddInclude(s => s.Grade!);
            AddInclude(s => s.Lessons!);
            AddInclude(s => s.Teacher!);
        }


        public SubjectSpecificationWithGradeAndLessonAndTeacher(SubjectFilteration subjectFilteration)
        {
            AddInclude(t => t.Grade!);
            AddInclude(t => t.Lessons!);
            AddInclude(t => t.Teacher!);
            //sorting 
            Sorting(subjectFilteration);

            if (subjectFilteration.PageSize > 0 && subjectFilteration.PageIndex > 0)
            {
                ApplyPaging(subjectFilteration.PageIndex, subjectFilteration.PageSize);
            }
        }
        
        private void Sorting(SubjectFilteration subjectFilteration)
        {
            switch (subjectFilteration.Sorting)
            {
                case SortingSubjects.SubjectNameAsc:
                    AddOrderBy(t => t.SubjectName);
                    break;
                case SortingSubjects.SubjectNameDesc:
                    AddOrderByDescending(t => t.SubjectName);
                    break;
                case SortingSubjects.CreatedAtAsc:
                    AddOrderBy(t => t.CreatedAt);
                    break;
                case SortingSubjects.CreatedAtDesc:
                    AddOrderByDescending(t => t.CreatedAt);
                    break;
                default:
                    AddOrderBy(t => t.Code);
                    break;
            }
        }
    }
}
