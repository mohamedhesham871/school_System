using Domain.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data
{
    public class StudentConfigurations : IEntityTypeConfiguration<Students>
    {
        public void Configure(EntityTypeBuilder<Students> builder)
        {
           builder.Property(s=>s.AssignToSchool).IsRequired().HasColumnType("date");
            builder.Property(s => s.Status)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValue("Active");
            builder.Property(s => s.ParentName).IsRequired();
            builder.Property(s => s.ParentContact)
                    .IsRequired()
                    .HasMaxLength(11); // Assuming a max length for contact number
           
            //Later Want to Add Relation with Grade and Class
            builder.HasOne(s=>s.Grade)
                   .WithMany(g => g.Students)
                   .HasForeignKey(s => s.GradeID)
                   .OnDelete(DeleteBehavior.NoAction);
            //Class Relation
            builder.HasOne(s => s.Class)
                   .WithMany(c => c.Students)
                   .HasForeignKey(s => s.ClassID)
                   .OnDelete(DeleteBehavior.NoAction);
            //Relation  With Subject In Table M:M [StudentAssignInSubject]
           
        }
    }
}
