
using Job_Portal_API.Context;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models;
using Job_Portal_API.Repositories;
using Job_Portal_API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Job_Portal_API
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
            builder.Services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
            });
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                 {
                     ValidateIssuer = false,
                     ValidateAudience = false,
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey:JWT"]))
                 };

             });

            #region Context
            builder.Services.AddDbContext<JobPortalApiContext>();
            #endregion

            #region Repositories
            builder.Services.AddScoped<IRepository<int, User>, UserRepository>();
            builder.Services.AddScoped<IRepository<int, Employer>, EmployerRepository>();
            builder.Services.AddScoped<IRepository<int, JobListing>, JobListingRepository>();
            builder.Services.AddScoped<IRepository<int, JobSeeker>, JobSeekerRepository>();
            builder.Services.AddScoped<IRepository<int, JobSeekerEducation>,EducationRepository>();
            builder.Services.AddScoped<IRepository<int, JobSeekerExperience>, ExperienceRepository>();
            builder.Services.AddScoped<IRepository<int, Application>, ApplicationRepository>();
            builder.Services.AddScoped<IRangeRepository<int, JobSeekerSkill>, JobSeekerSkillRepository>();
            builder.Services.AddScoped<IRepository<int, JobSkill>, JobSkillRepository>();

            #endregion

            #region Services
            builder.Services.AddScoped<IUser, UserService>();
            builder.Services.AddScoped<IToken, TokenService>();
            builder.Services.AddScoped<IEmployer, EmployerService>();
            builder.Services.AddScoped<IJobListing, JobListingService>();
            builder.Services.AddScoped<IJobSeeker, JobSeekerService>();
            builder.Services.AddScoped<IApplication, ApplicationService>();
            builder.Services.AddScoped<IAdmin, AdminService>();
            builder.Services.AddSingleton(new EmailService("smtp.gmail.com", 587, "amanagrawal2001.am.aa@gmail.com", "upjufivltdryzmyj"));
            #endregion
            #region CORS
            builder.Services.AddCors(opts =>
            {
                opts.AddPolicy("MyCors", options =>
                {
                    options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("MyCors");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
