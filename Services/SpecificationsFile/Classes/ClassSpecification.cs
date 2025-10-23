using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.SpecificationsFile.Classes
{
    public  class ClassSpecification:Specifications<ClassEntity>
    {

        public ClassSpecification(string classCode):base(c=>c.Code==classCode)
        {
            AddInclude(c => c.Grade!);
        }


        public ClassSpecification(ClassFilteration classFilteration) : base(c=>(string.IsNullOrEmpty(classFilteration.GradeCode) ||
         c.Grade.GradeCode == classFilteration.GradeCode) &&
        (string.IsNullOrEmpty(classFilteration.SearchKey) ||
         c.ClassName.Contains(classFilteration.SearchKey)))
        {
            AddInclude(c => c.Grade!);

            Sorting(classFilteration);

            if (classFilteration.PageIndex <= 0) classFilteration.PageIndex = 1;
            
                ApplyPaging(classFilteration.PageIndex,classFilteration.PageSize);
        }

        private void Sorting(ClassFilteration f)
        {
            switch (f.Sorting) 
            {
                case SortingClass.NameAsc:
                    AddOrderBy(s => s.ClassName);
                    break;
                case SortingClass.NameDesc:
                    AddOrderByDescending(s=> s.ClassName);
                    break;
                case SortingClass.CreateAtAsc:
                    AddOrderByDescending(s => s.CreateAt);
                    break;
                case SortingClass.CreateAtDesc:
                    AddOrderBy(s => s.CreateAt);
                    break;
                case SortingClass.LastUpdateAtAsc:
                    AddOrderByDescending(s=>s.UpdateAt);
                    break;
                case SortingClass.LastUpdateAtDesc:
                    AddOrderBy(s => s.UpdateAt);
                    break;
                default
                    :AddOrderBy(s => s.ClassName);
                    break;
            }
        }
    }
}
