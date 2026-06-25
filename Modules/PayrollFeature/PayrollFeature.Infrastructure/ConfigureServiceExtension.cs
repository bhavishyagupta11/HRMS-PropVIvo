using Microsoft.Extensions.DependencyInjection;
using PayrollFeature.Application.Repository;
using PayrollFeature.Infrastructure.Repositories;

namespace PayrollFeature.Infrastructure
{
    public static class ConfigureServiceExtension
    {
        public static IServiceCollection AddPayrollInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IPayrollRepository, PayrollRepository>();
            services.AddSingleton<HRMS.Core.Postgres.Interfaces.IPostgresEntityConfigurator, PayrollRecordConfigurator>();
            return services;
        }
    }
}
