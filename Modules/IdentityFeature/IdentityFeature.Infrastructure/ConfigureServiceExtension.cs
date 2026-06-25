using HRMS.Core.Postgres.Interfaces;
using IdentityFeature.Application.Repository;
using IdentityFeature.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace IdentityFeature.Infrastructure
{
    public static class ConfigureServiceExtension
    {
        public static IServiceCollection AddIdentityDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddSingleton<IPostgresEntityConfigurator, IdentityEntityConfigurator>();
            services.AddSingleton<IdentityFeature.Application.Interfaces.IPasswordHasher, IdentityFeature.Infrastructure.Services.PasswordHasher>();
            services.AddScoped<IdentityFeature.Application.Interfaces.ITokenService, IdentityFeature.Infrastructure.Services.JwtTokenService>();
            services.AddScoped<Microsoft.AspNetCore.Identity.IPasswordHasher<IdentityFeature.Domain.User>, Microsoft.AspNetCore.Identity.PasswordHasher<IdentityFeature.Domain.User>>();
            services.AddHostedService<IdentityFeature.Infrastructure.Services.IdentitySeeder>();

            var jwtKey = configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey)) throw new System.ArgumentNullException("Jwt:Key is missing from configuration.");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"] ?? "hrms_issuer",
                        ValidAudience = configuration["Jwt:Audience"] ?? "hrms_audience",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                    };
                });
            services.AddAuthorization();

            return services;
        }
    }
}
