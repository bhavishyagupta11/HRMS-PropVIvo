using AnnouncementFeature.Application.Repository;
using AnnouncementFeature.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AnnouncementFeature.Infrastructure
{
    public static class ConfigureServiceExtension
    {
        public static IServiceCollection AddAnnouncementInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
            services.AddSingleton<HRMS.Core.Postgres.Interfaces.IPostgresEntityConfigurator, AnnouncementEntityConfigurator>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AnnouncementFeature.Application.DTO.AnnouncementRecordDto).Assembly));
            return services;
        }
    }
}
