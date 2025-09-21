using Domain.Models.User;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public  class Lesson
    {
        public int LessonId { get; set; }   // Primary Key
        public string LessonCode { get; set; } = string.Empty; // Unique code for the lesson (e.g., "MATH-FRACTIONS-101")
        // Core Info
        public string Title { get; set; } = string.Empty;   // Lesson title (e.g., "Math - Fractions")
        public string Description { get; set; } = string.Empty; // Optional details
        public string? MaterialUrl { get; set; }  // Link to files, PDFs, videos
        public bool IsActive { get; set; } = true;  //Means if the lesson is currently active or archived
        public int Order { get; set; }        //Order of the lesson in the subject
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt {  get; set; } = DateTime.Now;

        //Relationships
        public int SubjectId { get; set; }   // FK → Subject
        public Subject Subject { get; set; } = null!;  // Navigation to Subject

    }
}

   