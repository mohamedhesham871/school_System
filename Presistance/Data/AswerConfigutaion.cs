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
    public class AswerConfigutaion : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.HasKey(A => A.Id);
            builder.HasIndex(A=> A.Code).IsUnique();
            builder.Property(A => A.AnswerText).IsRequired();

            builder.HasOne(A=>A.Question)
                   .WithMany()
                   .HasForeignKey(A=> A.QuestionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
