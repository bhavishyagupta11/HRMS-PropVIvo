using PerformanceFeature.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PerformanceFeature.Application.Repository
{
    public interface IPerformanceRepository
    {
        Task<IEnumerable<GoalRecord>> GetGoalsAsync(string userId);
        Task<IEnumerable<PerformanceReviewRecord>> GetReviewsAsync(string userId);
        Task CreateGoalAsync(GoalRecord record);
        Task CreateReviewAsync(PerformanceReviewRecord record);
    }
}
