using AnalyticsFeature.Application.Repository;
using AnalyticsFeature.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AnalyticsFeature.Infrastructure
{
    public static class ConfigureServiceExtension
    {
        public static IServiceCollection AddAnalyticsInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IAnalyticsRepository, AnalyticsRepository>();
            services.AddSingleton<HRMS.Core.Postgres.Interfaces.IPostgresEntityConfigurator, AnalyticsEntityConfigurator>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AnalyticsFeature.Application.DTO.AnalyticsDashboardDto).Assembly));
            return services;
        }
    }
}
