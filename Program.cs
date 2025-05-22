using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using UserRegistrationSystem.Application.Interfaces;
using UserRegistrationSystem.Application.Services;
using UserRegistrationSystem.Infrastructure.Persistence;

namespace UserRegistrationSystem.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ---------------------------------------
            // 🔧 1. Add DbContext
            // ---------------------------------------
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    sql => sql.MigrationsAssembly("UserRegistrationSystem.Infrastructure")
                )
            );
            // ---------------------------------------
            // 📦 2. Register Services
            // ---------------------------------------
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IAdminService, AdminService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AlloAll", policy =>
               {
                   policy.WithOrigins("http://localhost:5215", "https://localhost:7000") // Leiskite šiems prievadams
                         .AllowAnyMethod()
                         .AllowAnyHeader();
               });
            });
            // ---------------------------------------
            // 🔐 3. Add JWT Authentication
            // ---------------------------------------

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(options =>
                 {
                     options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                     {
                         ValidateIssuer = true,
                         ValidateAudience = true,
                         ValidateLifetime = true,
                         ValidateIssuerSigningKey = true,
                         ValidIssuer = builder.Configuration["Jwt:Issuer"],
                         ValidAudience = builder.Configuration["Jwt:Audience"],
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                     };
                 });

           

            // ---------------------------------------
            // 📚 4. Add Controllers and Swagger
            // ---------------------------------------
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
               options.SwaggerDoc("v1", new OpenApiInfo
               {
                   Title = "UserRegistrationSystem", 
                    Version = "v1"
               });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
               options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                       new string[] {}
                  }
               });
            });

            // ---------------------------------------
            // 🔒 5. Add Authorization
            // ---------------------------------------
            builder.Services.AddAuthorization();

            // ---------------------------------------
            // 🚀 6. Build and Run
            // ---------------------------------------
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API Title v1");
                });

            }

            app.UseHttpsRedirection();

            app.UseCors("AlloAll");

            app.UseAuthentication(); 
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
