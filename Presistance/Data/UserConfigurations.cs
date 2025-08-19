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
    public class UserConfigurations : IEntityTypeConfiguration<Students>
    {
        public void Configure(EntityTypeBuilder<Students> builder)
        {
           builder.Property(s=>s.EnrollmentDate).IsRequired().HasColumnType("date");
            builder.Property(s => s.Status)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValue("Active");
            builder.Property(s => s.ParentName).IsRequired();
            builder.Property(s => s.ParentContact)
                    .IsRequired()
                    .HasMaxLength(11); // Assuming a max length for contact number
            //Later Want to Add Relation with Grade and Class
        //    builder.HasOne<AppUsers>()
        //           .WithMany()
        //           .HasForeignKey(s => s.Id) // Assuming Id is the foreign key
        //           .OnDelete(DeleteBehavior.SetNull); // Adjust delete behavior as needed
        //
        }
    }
}
