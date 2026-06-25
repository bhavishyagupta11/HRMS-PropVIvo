using HRMS.Core.Postgres.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AttendanceFeature.Domain.Repositories;
using AttendanceFeature.Infrastructure.Repositories;

namespace AttendanceFeature.Infrastructure
{
    public static class ConfigureServiceExtension
    {
        public static IServiceCollection AddAttendanceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAttendanceRepository, AttendanceRepository>();
            services.AddSingleton<IPostgresEntityConfigurator, AttendanceEntityConfigurator>();
            return services;
        }
    }
}
