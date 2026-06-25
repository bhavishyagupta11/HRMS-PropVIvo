using AnalyticsFeature.Application.DTO;
using AnalyticsFeature.Application.Repository;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AnalyticsFeature.Application.Queries
{
    public class GetAnalyticsDashboardQuery : IRequest<AnalyticsDashboardDto>
    {
    }

    public class GetAnalyticsDashboardQueryHandler : IRequestHandler<GetAnalyticsDashboardQuery, AnalyticsDashboardDto>
    {
        private readonly IAnalyticsRepository _repository;

        public GetAnalyticsDashboardQueryHandler(IAnalyticsRepository repository)
        {
            _repository = repository;
        }

        public async Task<AnalyticsDashboardDto> Handle(GetAnalyticsDashboardQuery request, CancellationToken cancellationToken)
        {
            var metrics = await _repository.GetMetricsAsync();
            var trends = await _repository.GetHeadcountTrendsAsync();
            var depts = await _repository.GetDepartmentDistributionsAsync();

            return new AnalyticsDashboardDto
            {
                Metrics = metrics.Select(x => new AnalyticsMetricRecordDto
                {
                    Id = x.Id,
                    MetricName = x.MetricName,
                    MetricValue = x.MetricValue,
                    Description = x.Description,
                    PercentageChange = x.PercentageChange,
                    CalculatedDate = x.CalculatedDate
                }),
                HeadcountTrends = trends.Select(x => new HeadcountTrendRecordDto
                {
                    Id = x.Id,
                    MonthYear = x.MonthYear,
                    TotalEmployees = x.TotalEmployees,
                    NewHires = x.NewHires,
                    Departures = x.Departures
                }),
                DepartmentDistributions = depts.Select(x => new DepartmentDistributionRecordDto
                {
                    Id = x.Id,
                    DepartmentName = x.DepartmentName,
                    EmployeeCount = x.EmployeeCount,
                    BudgetUtilization = x.BudgetUtilization
                })
            };
        }
    }
}
