using Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class StudentAssignInSubject
    {
        public string StudentId { get; set; } = null!;
        public int SubjectId { get; set; }

        public string SubjectName { get; set; } = null!;
        public string UserName { get; set; } = null!;

        public Students Student { get; set; } = null!;
        public Subject Subject { get; set; } = null!;

    }
}
