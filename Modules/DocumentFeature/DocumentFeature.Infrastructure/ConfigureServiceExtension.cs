using DocumentFeature.Application.Repository;
using DocumentFeature.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentFeature.Infrastructure
{
    public static class ConfigureServiceExtension
    {
        public static IServiceCollection AddDocumentInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddSingleton<HRMS.Core.Postgres.Interfaces.IPostgresEntityConfigurator, DocumentEntityConfigurator>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DocumentFeature.Application.DTO.DocumentRecordDto).Assembly));
            return services;
        }
    }
}
