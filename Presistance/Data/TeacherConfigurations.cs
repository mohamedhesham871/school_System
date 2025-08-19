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
         

        }
    }
}
