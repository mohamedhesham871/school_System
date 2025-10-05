using Domain.Models.User;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.subject_Lesson;

namespace Domain.Models
{
    public  class Lesson:IHasCode
    {
        public int LessonId { get; set; }   // Primary Key
        public string Code { get; set; } // Unique code for the lesson (e.g., "MATH-FRACTIONS-101")
        // Core Info
        public string Title { get; set; } = string.Empty;   // Lesson title (e.g., "Math - Fractions")
        public string Description { get; set; } = string.Empty; // Optional details
        public string? MaterialUrl { get; set; }  // Link to files, PDFs, videos
        public bool IsActive { get; set; } = true;  //Means if the lesson is currently active or archived
        public int Order { get; set; }        //Order of the lesson in the subject
        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt {  get; set; } 

        //Relationships
       
        public int SubjectId { get; set; }   // FK → Subject
        public Subject Subject { get; set; } = null!;  // Navigation to Subject
        
        public Quiz? quiz { get; set; }
        
        
        public Lesson()
        {
            Code = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

    }
}

   