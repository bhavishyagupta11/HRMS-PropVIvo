using HRMS.Core.Postgres.Data;
using HRMS.Core.Postgres.Repositories;
using HRMS.Core.Telemetry;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnboardingFeature.Application.Repository;
using OnboardingFeature.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingFeature.Infrastructure.Repositories
{
    public class OnboardingRepository : PostgresDbRepository<OnboardingEmployee>, IOnboardingRepository
    {
        private readonly PostgresDbContext _ctx;

        public OnboardingRepository(
            PostgresDbContext context,
            ILogger<OnboardingRepository> logger,
            ITelemetryService telemetryService,
            IHttpContextAccessor httpContextAccessor)
            : base(context, logger, telemetryService, httpContextAccessor)
        {
            _ctx = context;
        }

        public override string TableName { get; } = "OnboardingEmployees";

        public override string GenerateId(OnboardingEmployee entity) => Guid.NewGuid().ToString();

        public async Task<OnboardingEmployee?> GetByUserIdAsync(string userId)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.UserId == userId);
        }

        public async Task<List<OnboardingTask>> GetTasksByEmployeeIdAsync(string onboardingEmployeeId)
        {
            return await _ctx.Set<OnboardingTask>()
                .AsNoTracking()
                .Where(t => t.OnboardingEmployeeId == onboardingEmployeeId)
                .ToListAsync();
        }

        public async Task<OnboardingTask?> GetTaskByIdAsync(string taskId)
        {
            return await _ctx.Set<OnboardingTask>().FindAsync(taskId);
        }

        public async Task<List<WelcomeMessage>> GetWelcomeMessagesByEmployeeIdAsync(string onboardingEmployeeId)
        {
            return await _ctx.Set<WelcomeMessage>()
                .AsNoTracking()
                .Where(m => m.OnboardingEmployeeId == onboardingEmployeeId)
                .ToListAsync();
        }

        public async Task<RelocationSupport?> GetRelocationSupportByEmployeeIdAsync(string onboardingEmployeeId)
        {
            return await _ctx.Set<RelocationSupport>()
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.OnboardingEmployeeId == onboardingEmployeeId);
        }

        public async Task<List<TeamIntroduction>> GetTeamIntroductionsByEmployeeIdAsync(string onboardingEmployeeId)
        {
            return await _ctx.Set<TeamIntroduction>()
                .AsNoTracking()
                .Where(i => i.OnboardingEmployeeId == onboardingEmployeeId)
                .ToListAsync();
        }

        public async Task<List<OnboardingMilestone>> GetMilestonesByEmployeeIdAsync(string onboardingEmployeeId)
        {
            return await _ctx.Set<OnboardingMilestone>()
                .AsNoTracking()
                .Where(m => m.OnboardingEmployeeId == onboardingEmployeeId)
                .ToListAsync();
        }

        public async Task<List<OnboardingTrainingModuleRef>> GetTrainingModuleRefsByEmployeeIdAsync(string onboardingEmployeeId)
        {
            return await _ctx.Set<OnboardingTrainingModuleRef>()
                .AsNoTracking()
                .Where(t => t.OnboardingEmployeeId == onboardingEmployeeId)
                .ToListAsync();
        }

        public async Task<OnboardingTask> UpdateTaskAsync(OnboardingTask task)
        {
            _ctx.Set<OnboardingTask>().Update(task);
            await _ctx.SaveChangesAsync();
            return task;
        }

        public async Task<OnboardingEmployee> UpdateEmployeeAsync(OnboardingEmployee employee)
        {
            _dbSet.Update(employee);
            await _ctx.SaveChangesAsync();
            return employee;
        }
    }
}
