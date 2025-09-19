using Domain.Models;
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
    public class TeacherConfigurations : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.Property(builder => builder.HiringDate)
                .IsRequired()
                .HasColumnType("date");
            builder.Property(builder => builder.Specialization).IsRequired();
            //Remember To Set Relations with Classes and Subjects

            //1- Class With Teacher in OnModelCreateing As New Table Because Relation is M:N 

            //2- Relation With Subjects
            builder.HasMany(s => s.Subjects)
                .WithOne(t => t.Teacher)
                .HasForeignKey(t => t.TeacherId)
                .OnDelete(DeleteBehavior.SetNull);
                
                
        }
    }
}
