using CopilotFeature.Application.Repository;
using CopilotFeature.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CopilotFeature.Infrastructure
{
    public static class ConfigureServiceExtension
    {
        public static IServiceCollection AddCopilotInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ICopilotRepository, CopilotRepository>();
            services.AddSingleton<HRMS.Core.Postgres.Interfaces.IPostgresEntityConfigurator, CopilotEntityConfigurator>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CopilotFeature.Application.DTO.CopilotInteractionRecordDto).Assembly));
            return services;
        }
    }
}
