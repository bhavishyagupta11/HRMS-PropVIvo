using System;
using System.Collections.Generic;

namespace AnalyticsFeature.Application.DTO
{
    public class AnalyticsMetricRecordDto
    {
        public string Id { get; set; } = string.Empty;
        public string MetricName { get; set; } = string.Empty;
        public string MetricValue { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PercentageChange { get; set; } = string.Empty;
        public DateTime CalculatedDate { get; set; }
    }

    public class HeadcountTrendRecordDto
    {
        public string Id { get; set; } = string.Empty;
        public string MonthYear { get; set; } = string.Empty;
        public int TotalEmployees { get; set; }
        public int NewHires { get; set; }
        public int Departures { get; set; }
    }

    public class DepartmentDistributionRecordDto
    {
        public string Id { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public int EmployeeCount { get; set; }
        public string BudgetUtilization { get; set; } = string.Empty;
    }

    public class AnalyticsDashboardDto
    {
        public IEnumerable<AnalyticsMetricRecordDto> Metrics { get; set; } = new List<AnalyticsMetricRecordDto>();
        public IEnumerable<HeadcountTrendRecordDto> HeadcountTrends { get; set; } = new List<HeadcountTrendRecordDto>();
        public IEnumerable<DepartmentDistributionRecordDto> DepartmentDistributions { get; set; } = new List<DepartmentDistributionRecordDto>();
    }
}
