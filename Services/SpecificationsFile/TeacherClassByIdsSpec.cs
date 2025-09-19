using Domain.Contract;
using Domain.Exceptions;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.SpecificationsFile
{
    public sealed class TeacherClassByIdsSpec : Specifications<TeacherClass>
    {
        public TeacherClassByIdsSpec(string teacherId, int classId) : base(tc => tc.TeacherId == teacherId && tc.ClassID == classId)
        {

        }
    }
}
