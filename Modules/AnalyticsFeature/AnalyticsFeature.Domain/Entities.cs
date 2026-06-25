using HRMS.Core.Postgres.Common;
using System;

namespace AnalyticsFeature.Domain
{
    public class AnalyticsMetricRecord : BaseEntity
    {
        public string MetricName { get; set; } = string.Empty;
        public string MetricValue { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PercentageChange { get; set; } = string.Empty;
        public DateTime CalculatedDate { get; set; }
    }

    public class HeadcountTrendRecord : BaseEntity
    {
        public string MonthYear { get; set; } = string.Empty;
        public int TotalEmployees { get; set; }
        public int NewHires { get; set; }
        public int Departures { get; set; }
    }

    public class DepartmentDistributionRecord : BaseEntity
    {
        public string DepartmentName { get; set; } = string.Empty;
        public int EmployeeCount { get; set; }
        public string BudgetUtilization { get; set; } = string.Empty;
    }
}
