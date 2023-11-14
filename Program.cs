using JWT.Demo.Configrations;
using JWT.Demo.Context;
using JWT.Demo.Helpers.Extensions;
using JWT.Demo.Helpers.GenericRepositories;
using JWT.Demo.Helpers.Methods;
using JWT.Demo.Helpers.UnitOfWork;
using JWT.Demo.Models.Entities;
using JWT.Demo.Services.AuthenticationServices;
using JWT.Demo.Services.EmployeeServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace JWT.Demo
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
            builder.Services.AddSwaggerConfiguration();

            builder.Services.Configure<Models.Authentication.JWT>(builder.Configuration.GetSection("JWT"));
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = builder.Configuration["JWT:Issuer"],
                        ValidAudience = builder.Configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<ICookieService, CookieService>();
            builder.Services.AddScoped<IGRepository<Employee>, GRepository<Employee>>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddSingleton<JWTConfig>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}