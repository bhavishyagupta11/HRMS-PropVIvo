using Microsoft.Extensions.DependencyInjection;
using TrainingFeature.Application.Repository;
using TrainingFeature.Infrastructure.Repositories;

namespace TrainingFeature.Infrastructure
{
    public static class ConfigureServiceExtension
    {
        public static IServiceCollection AddTrainingInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ITrainingRepository, TrainingRepository>();
            services.AddSingleton<HRMS.Core.Postgres.Interfaces.IPostgresEntityConfigurator, TrainingEntityConfigurator>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(TrainingFeature.Application.DTO.TrainingCourseRecordDto).Assembly));
            return services;
        }
    }
}
