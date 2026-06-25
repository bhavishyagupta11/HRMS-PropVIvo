using Microsoft.Extensions.DependencyInjection;
using RecognitionFeature.Application.Repository;
using RecognitionFeature.Infrastructure.Repositories;

namespace RecognitionFeature.Infrastructure
{
    public static class ConfigureServiceExtension
    {
        public static IServiceCollection AddRecognitionInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IRecognitionRepository, RecognitionRepository>();
            services.AddSingleton<HRMS.Core.Postgres.Interfaces.IPostgresEntityConfigurator, RecognitionEntityConfigurator>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RecognitionFeature.Application.DTO.RecognitionRecordDto).Assembly));
            return services;
        }
    }
}
