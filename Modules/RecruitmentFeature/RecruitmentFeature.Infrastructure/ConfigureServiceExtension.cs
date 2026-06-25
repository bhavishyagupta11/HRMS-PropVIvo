using Microsoft.Extensions.DependencyInjection;
using RecruitmentFeature.Application.Repository;
using RecruitmentFeature.Infrastructure.Repositories;

namespace RecruitmentFeature.Infrastructure
{
    public static class ConfigureServiceExtension
    {
        public static IServiceCollection AddRecruitmentInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IRecruitmentRepository, RecruitmentRepository>();
            services.AddSingleton<HRMS.Core.Postgres.Interfaces.IPostgresEntityConfigurator, RecruitmentEntityConfigurator>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RecruitmentFeature.Application.DTO.JobRequisitionRecordDto).Assembly));
            return services;
        }
    }
}
