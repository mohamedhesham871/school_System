using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data
{
    public class SubjectsConfiguration : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.HasKey(s=>s.SubjectID);
            builder.HasIndex(s => s.SubjectCode).IsUnique(); // Unique index
            builder.Property(s=>s.SubjectCode).IsRequired().HasMaxLength(20);
            builder.Property(s => s.SubjectName).IsRequired().HasMaxLength(100);
            builder.Property(s => s.Description).HasMaxLength(int.MaxValue);
            builder.Property(s => s.CreatedAt).HasDefaultValueSql("GETDATE()");
            builder.Property(s => s.UpdatedAt).HasDefaultValueSql("GETDATE()");
            // Relation with Teacher
            builder.HasOne(s => s.Teacher)
                   .WithMany(t => t.Subjects)
                   .HasForeignKey(s => s.TeacherId)
                   .OnDelete(DeleteBehavior.SetNull);
            // Relation with Grade
            builder.HasOne(s => s.Grade)
                   .WithMany(g => g.Subjects)
                   .HasForeignKey(s => s.GradeID)
                   .OnDelete(DeleteBehavior.Cascade);
            // Relation with Lesson 1:M [Subject has many Lessons] made in LessonConfiguration       


        }
    }
}
