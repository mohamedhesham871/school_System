using Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public  class Grade
    {
        public int ID { get; set; }
        public string Description { get; set; } = string.Empty;
        public string GradeCode { get; set; } = string.Empty;
        public string GradeName { get; set; } = string.Empty;
        public string AcademicYear { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        //Relation with Subject 1:M
        public ICollection<Subject> Subjects { get; set; } = [];
        public ICollection<Students> Students { get; set; } = [];
    }
}