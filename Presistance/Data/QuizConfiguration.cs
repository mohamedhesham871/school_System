using Domain.Models.subject_Lesson;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data
{
    public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            builder.HasKey(q => q.QuizId);
            builder.Property(q => q.Title).IsRequired().HasMaxLength(200);
            builder.Property(q => q.Description).HasMaxLength(1000);
            builder.Property(q => q.CreatedAt).HasDefaultValueSql("GETDATE()");
            builder.Property(q => q.UpdatedAt).HasDefaultValueSql("GETDATE()");
            builder.HasIndex(q => q.Code).IsUnique();
           
            builder.HasOne(q => q.Lesson)
                    .WithOne()
                    .HasForeignKey<Quiz>(q => q.LessonId)
                    .OnDelete(DeleteBehavior.Cascade);

            //Relation With Students M:N in OnModelCreating As New Table [StudentQuiz]
            //Relation With Questions 1:M in QuestionConfiguration


        }
    }
}
