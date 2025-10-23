using Domain.Models;
using Domain.Models.subject_Lesson;
using Domain.Models.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Contexts
{
    public  class SchoolDbContexts(DbContextOptions<SchoolDbContexts> options) : IdentityDbContext<AppUsers>(options)
    {
        protected override void OnModelCreating(ModelBuilder Builder)
        {

            
            Builder.ApplyConfigurationsFromAssembly(typeof(SchoolDbContexts).Assembly);
           //Relation Between Teacher and Classes
            Builder.Entity<TeacherClass>(e =>
            {
                e.ToTable("TeacherClass");
                e.HasKey(x => new { x.TeacherId, x.ClassID });

                e.HasOne(x => x.Teacher)
                 .WithMany(t => t.TeacherClasses)
                 .HasForeignKey(x => x.TeacherId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.Class)
                 .WithMany(c => c.TeacherClasses)
                 .HasForeignKey(x => x.ClassID)
                 .OnDelete(DeleteBehavior.Cascade);
            });
            //Relation Between Quiz and Students M:N will Generate New Table [StudentQuiz]
            Builder.Entity<QuizStudent>(e =>
            {
                e.ToTable("QuizStudent");
                e.HasKey(x => new { x.StudentId, x.QuizId });

                e.HasOne(x => x.Student)
                 .WithMany(s => s.QuizStudents)
                 .HasForeignKey(x => x.StudentId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.Quiz)
                 .WithMany(q => q.QuizStudents)
                 .HasForeignKey(x => x.QuizId)
                 .OnDelete(DeleteBehavior.Cascade);
            });
            base.OnModelCreating(Builder);

            //Releation Between Student And Subject M:N Will Generate New Table [StudentAssignInSubject]
            Builder.Entity<StudentAssignInSubject>(e =>
            {
                e.ToTable("StudentAssignInSubject");
                e.HasKey(x => new { x.StudentId, x.SubjectId });

                e.HasOne(x => x.Student)
                 .WithMany(s => s.StudentAssignInSubjects)
                 .HasForeignKey(x => x.StudentId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.Subject)
                 .WithMany(su => su.StudentAssignInSubjects)
                 .HasForeignKey(x => x.SubjectId)
                 .OnDelete(DeleteBehavior.Cascade);
            });
            // Additional configurations can be added here if needed
        }

        public DbSet<Students> Students { get; set; } = null!;
        public DbSet<Teacher> Teachers { get; set; } = null!;
        public DbSet<Grade> Grades { get; set; } = null!;
        public DbSet<Subject> Subjects { get; set; } = null!;
        public DbSet<Lesson> Lessons { get; set; } = null!;
        public DbSet<ClassEntity> StudentClasses { get; set; } = null!;
        public DbSet<Question> Questions { get; set; }
        public DbSet<Quiz> Quizzes { get; set; } = null!;
        public DbSet<Answer> Answers { get; set; } = null!;
    }
}
