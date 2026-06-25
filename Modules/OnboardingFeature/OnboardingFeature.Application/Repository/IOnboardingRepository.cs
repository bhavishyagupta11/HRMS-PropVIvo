using HRMS.Core.Postgres.Repositories;
using OnboardingFeature.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnboardingFeature.Application.Repository
{
    public interface IOnboardingRepository : IPostgresRepository<OnboardingEmployee>
    {
        Task<OnboardingEmployee?> GetByUserIdAsync(string userId);
        Task<List<OnboardingTask>> GetTasksByEmployeeIdAsync(string onboardingEmployeeId);
        Task<OnboardingTask?> GetTaskByIdAsync(string taskId);
        Task<List<WelcomeMessage>> GetWelcomeMessagesByEmployeeIdAsync(string onboardingEmployeeId);
        Task<RelocationSupport?> GetRelocationSupportByEmployeeIdAsync(string onboardingEmployeeId);
        Task<List<TeamIntroduction>> GetTeamIntroductionsByEmployeeIdAsync(string onboardingEmployeeId);
        Task<List<OnboardingMilestone>> GetMilestonesByEmployeeIdAsync(string onboardingEmployeeId);
        Task<List<OnboardingTrainingModuleRef>> GetTrainingModuleRefsByEmployeeIdAsync(string onboardingEmployeeId);
        Task<OnboardingTask> UpdateTaskAsync(OnboardingTask task);
        Task<OnboardingEmployee> UpdateEmployeeAsync(OnboardingEmployee employee);
    }
}
