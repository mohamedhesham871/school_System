using AbstractionServices;
using Domain.Contract;
using AutoMapper;
using Domain.Models.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Persistence.Contexts;
using Persistence.Repository;
using Services;
using Services.Profiles;
using Shared;
using System.Text;
using Microsoft.Extensions.Logging;

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
            builder.Services.AddScoped<IDbInitializer, DbInitializer>();
            builder.Services.AddScoped<IAuthServices, AuthServices>();
            builder.Services.AddScoped<ITeacherService, TeacherService>();
            builder.Services.AddScoped<ISubjectServices, SubjectServices>();
            builder.Services.AddScoped<ILessonServices, LessonServices>();
            builder.Services.AddAutoMapper(typeof(UsersProfile).Assembly);
            #endregion

            #region Identity services
            builder.Services.AddIdentity<AppUsers, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
            })
           .AddEntityFrameworkStores<SchoolDbContexts>()
           .AddDefaultTokenProviders();
            #endregion
            builder.Services.AddDbContext<SchoolDbContexts>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SchoolDbConnection")));

            #region Authorize And Authentication
            builder.Services.Configure<JwtToken>(builder.Configuration.GetSection("JwtOptions"));
            var JWTtoken = builder.Configuration.GetSection("JwtOptions").Get<JwtToken>();

            builder.Services.AddAuthentication(options =>
            {

                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(Options =>
                 Options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,


                     ValidIssuer = JWTtoken.Issuer,
                     ValidAudience = JWTtoken.Audience,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTtoken.SecretKey))
                 });
            builder.Services.AddAuthorization();//VIP to Authorize work


            #endregion
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            #region Seeding Data
             SeedingData(app).Wait();
            #endregion
           

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
           

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
        private async static Task SeedingData(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                var RoleSeeding = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
               
                await dbInitializer.Initialize();
                await dbInitializer.InitializeRole(RoleSeeding);
            }
        }
    }
}
