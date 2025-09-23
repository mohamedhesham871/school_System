using Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.subject_Lesson
{
    public  class QuizStudent
    {
        // Composite Key: StudentId + QuizId
        public string StudentId { get; set; } = string.Empty; // FK → Student (User)
        public int QuizId { get; set; }   // FK → Quiz

        // Code for use it instad of Composite Key
        public Guid StudentQuizCode { get; set; }

        public DateTime TakenAt { get; set; } 
        public double Score { get; set; }  // Score achieved in the quiz
        // if Student Get Less than 50% he will not pass the quiz
        public bool IsPassed { get; set; }
        // Navigation properties
        public Quiz Quiz { get; set; } = null!;  // Navigation to Quiz
        public Students Student { get; set; } = null!;  // Navigation to Student (User)

        public QuizStudent()
        {
            StudentQuizCode= Guid.NewGuid();
            TakenAt = DateTime.Now;
            Score = 0.0;
            IsPassed = false;
        }
    }
}
