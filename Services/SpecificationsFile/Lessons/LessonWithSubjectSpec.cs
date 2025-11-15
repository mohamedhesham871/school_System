using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.SpecificationsFile.Lessons
{
    public  class LessonWithSubjectSpec:Specifications<Lesson>
    {
        public LessonWithSubjectSpec(string lessonCode) :base(s=>s.Code==lessonCode)
        {
            AddInclude(s => s.Subject);
           
        }

    }
}
