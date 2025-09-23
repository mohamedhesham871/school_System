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
    public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {   
            builder.HasKey(l => l.LessonId);
            builder.HasIndex(l=>l.Code).IsUnique();

            builder.Property(l => l.Code).IsRequired().HasMaxLength(50);
            builder.Property(l => l.Title).IsRequired().HasMaxLength(200);
            builder.Property(l => l.Description).IsRequired();

            builder.Property(l => l.CreatedAt).HasDefaultValueSql("GETDATE()");
            builder.Property(l => l.UpdatedAt).HasDefaultValueSql("GETDATE()");
            // Relation with Subject
            builder.HasOne(l => l.Subject)
                   .WithMany(s => s.Lessons)
                   .HasForeignKey(l => l.SubjectId)
                   .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
