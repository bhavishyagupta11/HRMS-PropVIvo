using ExpenseFeature.Application.Repository;
using ExpenseFeature.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseFeature.Infrastructure
{
    public static class ConfigureServiceExtension
    {
        public static IServiceCollection AddExpenseInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddSingleton<HRMS.Core.Postgres.Interfaces.IPostgresEntityConfigurator, ExpenseEntityConfigurator>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ExpenseFeature.Application.DTO.ReimbursementRecordDto).Assembly));
            return services;
        }
    }
}
