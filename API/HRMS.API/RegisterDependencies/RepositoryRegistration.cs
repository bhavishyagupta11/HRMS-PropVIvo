using IdentityFeature.Infrastructure;
using OnboardingFeature.Infrastructure;
using AttendanceFeature.Infrastructure;

namespace HRMS.API.RegisterDependencies
{
    public static class RepositoryRegistration
    {
        public static IServiceCollection AddModulesDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
    }
}
