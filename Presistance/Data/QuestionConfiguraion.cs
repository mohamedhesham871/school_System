using Domain.Models.subject_Lesson;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data
{
    public class QuestionConfiguraion : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(q => q.QuestionId);
            builder.HasIndex(q => q.Code).IsUnique();
            builder.Property(q => q.QuestionText).IsRequired();
            builder.Property(q => q.QuestionType).IsRequired();
            builder.Property(q => q.CreatedAt).HasDefaultValueSql("GETDATE()");
            builder.Property(q => q.UpdatedAt).HasDefaultValueSql("GETDATE()");
            builder.HasMany(q => q.Answers)
                   .WithOne(a => a.Question)
                   .HasForeignKey(a => a.QuestionId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(q => q.Quiz)
                   .WithMany(quiz => quiz.Questions)
                   .HasForeignKey(q => q.QuizId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
