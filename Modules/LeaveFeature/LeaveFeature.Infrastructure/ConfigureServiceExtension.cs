using HRMS.Core.Postgres.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LeaveFeature.Application.Repository;
using LeaveFeature.Infrastructure.Repositories;

namespace LeaveFeature.Infrastructure
{
    public static class ConfigureServiceExtension
    {
        public static IServiceCollection AddLeaveInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ILeaveRepository, LeaveRepository>();
            services.AddSingleton<IPostgresEntityConfigurator, LeaveEntityConfigurator>();
            return services;
        }
    }
}
