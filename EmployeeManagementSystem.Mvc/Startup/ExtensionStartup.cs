using System.Text;
using DiÆon.Aggregator;
using EmployeeManagementSystem.Application;
using EmployeeManagementSystem.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

namespace EmployeeManagementSystem.Mvc.Startup;

internal static class ExtensionStartup
{
    public static IServiceCollection AggregateServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllersWithViews();
        services.AddEndpointsApiExplorer();
        
        #region Logger
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("../EmployeeManagementSystem.Infrastructure/DAL/Logs/log-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
        #endregion Logger
        
        #region Swagger
        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Employee Management System API",
                Version = "v1",
                Description = "RESTful API for Employee Management System"
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    Array.Empty<string>()
                }
            });
        });
        #endregion swagger
        
        #region Security
        
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });
        
        var jwtKey = configuration["Jwt:Key"] ?? "YourSecretKeyForJWTTokenGeneration123456789";
        var jwtIssuer = configuration["Jwt:Issuer"] ?? "EmployeeManagementSystem";
        var jwtAudience = configuration["Jwt:Audience"] ?? "EmployeeManagementSystem";

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });
        services.AddAuthorization();
        #endregion Security
        
        #region Modules
        services.AggregateLifeTime(typeof(ApplicationAssembly).Assembly, typeof(InfrastructureAssembly).Assembly);;
        services.AddInfrastructure(configuration);
        #endregion Modules
        
        return services;
    }
}