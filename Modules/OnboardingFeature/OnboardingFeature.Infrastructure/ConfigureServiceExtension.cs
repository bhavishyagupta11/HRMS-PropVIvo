using HRMS.Core.Postgres.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnboardingFeature.Application.Repository;
using OnboardingFeature.Infrastructure.Repositories;

namespace OnboardingFeature.Infrastructure
{
    public static class ConfigureServiceExtension
    {
        public static IServiceCollection AddOnboardingDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IOnboardingRepository, OnboardingRepository>();
            services.AddSingleton<IPostgresEntityConfigurator, OnboardingEntityConfigurator>();
            return services;
        }
    }
}
