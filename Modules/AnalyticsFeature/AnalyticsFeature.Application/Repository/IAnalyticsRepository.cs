using AnalyticsFeature.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalyticsFeature.Application.Repository
{
    public interface IAnalyticsRepository
    {
        Task<IEnumerable<AnalyticsMetricRecord>> GetMetricsAsync();
        Task<IEnumerable<HeadcountTrendRecord>> GetHeadcountTrendsAsync();
        Task<IEnumerable<DepartmentDistributionRecord>> GetDepartmentDistributionsAsync();
    }
}
