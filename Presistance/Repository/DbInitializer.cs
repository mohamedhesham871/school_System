using Domain.Contract;
using Domain.Models;
using Domain.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public class DbInitializer(SchoolDbContexts context) : IDbInitializer
    {
        public async Task Initialize()
        {
            // Check if there is migrations that have not been applied yet
            //Note that We using this in Deployment not Development
            if ((await context.Database.GetAppliedMigrationsAsync()).Any())
            {
                await context.Database.MigrateAsync();
            }
            // Seed the database with initial data if necessary
            try
            {
                if (!await context.Grades.AnyAsync())
                {
                    // var data = await File.ReadAllTextAsync(@"..\interface\Presistance\SeedingData\Seeding_Grade.Json");
                    var data = await File.ReadAllTextAsync(@"..\Presistance\SeedingData\Seeding_Grade.Json");
                    var grades = JsonSerializer.Deserialize<List<Grade>>(data);
                    if (grades != null && grades.Count > 0)
                    {
                        await context.Grades.AddRangeAsync(grades);
                    }
                    await context.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
            }

        }

        public async Task InitializeRole(RoleManager<IdentityRole> roleSeeding)
        {
            try
            {
                var Roles = new[] { "Admin", "Teacher", "Student" };
                foreach (var role in Roles)
                {
                    if (!await roleSeeding.RoleExistsAsync(role))
                    {
                        await roleSeeding.CreateAsync(new IdentityRole(role));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");

            }
        }
    }
}
