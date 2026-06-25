using HRMS.Core.Postgres.Data;
using Microsoft.EntityFrameworkCore;
using PerformanceFeature.Application.Repository;
using PerformanceFeature.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerformanceFeature.Infrastructure.Repositories
{
    public class PerformanceRepository : IPerformanceRepository
    {
        private readonly PostgresDbContext _dbContext;

        public PerformanceRepository(PostgresDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<GoalRecord>> GetGoalsAsync(string userId)
        {
            return await _dbContext.Set<GoalRecord>()
                .Where(x => x.UserId == userId)
                .OrderBy(x => x.TargetDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<PerformanceReviewRecord>> GetReviewsAsync(string userId)
        {
            return await _dbContext.Set<PerformanceReviewRecord>()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.ReviewCycle)
                .ToListAsync();
        }

        public async Task CreateGoalAsync(GoalRecord record)
        {
            await _dbContext.Set<GoalRecord>().AddAsync(record);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateReviewAsync(PerformanceReviewRecord record)
        {
            await _dbContext.Set<PerformanceReviewRecord>().AddAsync(record);
            await _dbContext.SaveChangesAsync();
        }
    }
}
