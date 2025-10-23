using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.SpecificationsFile.Subjects
{
    internal class SubjectDuplicationCheck : Specifications<Subject>
    {
        public SubjectDuplicationCheck(string SCode) : base(s => s.Code == SCode)
        {

        }
    }
}
