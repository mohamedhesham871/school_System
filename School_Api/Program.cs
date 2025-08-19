
using Domain.Contract;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using Persistence.Repository;

namespace School_Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            #region Register Repositories
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            #endregion
            builder.Services.AddDbContext<SchoolDbContexts>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SchoolDbConnection")));
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
