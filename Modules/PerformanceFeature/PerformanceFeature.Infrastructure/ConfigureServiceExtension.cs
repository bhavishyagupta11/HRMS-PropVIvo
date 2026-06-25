using Microsoft.Extensions.DependencyInjection;
using PerformanceFeature.Application.Repository;
using PerformanceFeature.Infrastructure.Repositories;

namespace PerformanceFeature.Infrastructure
{
    public static class ConfigureServiceExtension
    {
        public static IServiceCollection AddPerformanceInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IPerformanceRepository, PerformanceRepository>();
            services.AddSingleton<HRMS.Core.Postgres.Interfaces.IPostgresEntityConfigurator, PerformanceEntityConfigurator>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(PerformanceFeature.Application.DTO.GoalRecordDto).Assembly));
            return services;
        }
    }
}
