using ContributionsFeature.Application.Repository;
using ContributionsFeature.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ContributionsFeature.Infrastructure
{
    public static class ConfigureServiceExtension
    {
        public static IServiceCollection AddContributionInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IContributionRepository, ContributionRepository>();
            services.AddSingleton<HRMS.Core.Postgres.Interfaces.IPostgresEntityConfigurator, ContributionEntityConfigurator>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ContributionsFeature.Application.DTO.ValueContributionDto).Assembly));
            return services;
        }
    }
}
