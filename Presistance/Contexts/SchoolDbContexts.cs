using Domain.Models;
using Domain.Models.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Contexts
{
    public  class SchoolDbContexts(DbContextOptions<SchoolDbContexts> options) : IdentityDbContext<AppUsers>(options)
    {
        protected override void OnModelCreating(ModelBuilder Builder)
        {

            
            Builder.ApplyConfigurationsFromAssembly(typeof(SchoolDbContexts).Assembly);
            base.OnModelCreating(Builder);
            // Additional configurations can be added here if needed
        }

        public DbSet<Students> Students { get; set; } = null!;
        public DbSet<Teacher> Teachers { get; set; } = null!;
        public DbSet<Grade> Grades { get; set; } = null!;

    }
}
