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

        // Core Info
        public string Title { get; set; } = string.Empty;   // Lesson title (e.g., "Math - Fractions")
        public string Description { get; set; } = string.Empty; // Optional details
        public int SubjectId { get; set; }   // FK → Subject
        public string? MaterialUrl { get; set; }  // Link to files, PDFs, videos
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}