using AnalyticsFeature.Application.Repository;
using AnalyticsFeature.Domain;
using HRMS.Core.Postgres.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnalyticsFeature.Infrastructure.Repositories
{
    public class AnalyticsRepository : IAnalyticsRepository
    {
        private readonly PostgresDbContext _dbContext;

        public AnalyticsRepository(PostgresDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<AnalyticsMetricRecord>> GetMetricsAsync()
        {
            var metrics = await _dbContext.Set<AnalyticsMetricRecord>().ToListAsync();
            if (!metrics.Any())
            {
                var seedMetrics = new List<AnalyticsMetricRecord>
                {
                    new AnalyticsMetricRecord { Id = "MET-1", MetricName = "Total Headcount", MetricValue = "1,428", Description = "Active global employees across all regions", PercentageChange = "+8.4% YoY", CalculatedDate = DateTime.UtcNow },
                    new AnalyticsMetricRecord { Id = "MET-2", MetricName = "Average Tenure", MetricValue = "4.2 Years", Description = "Exceeds software industry benchmarks", PercentageChange = "+1.2 yrs", CalculatedDate = DateTime.UtcNow },
                    new AnalyticsMetricRecord { Id = "MET-3", MetricName = "Monthly Attrition", MetricValue = "0.8%", Description = "Voluntary turnover rate for current month", PercentageChange = "-0.3%", CalculatedDate = DateTime.UtcNow },
                    new AnalyticsMetricRecord { Id = "MET-4", MetricName = "Attendance & Compliance", MetricValue = "98.4%", Description = "Overall workplace schedule fulfillment", PercentageChange = "+0.5%", CalculatedDate = DateTime.UtcNow }
                };
                await _dbContext.Set<AnalyticsMetricRecord>().AddRangeAsync(seedMetrics);
                await _dbContext.SaveChangesAsync();
                return seedMetrics;
            }
            return metrics;
        }

        public async Task<IEnumerable<HeadcountTrendRecord>> GetHeadcountTrendsAsync()
        {
            var trends = await _dbContext.Set<HeadcountTrendRecord>().ToListAsync();
            if (!trends.Any())
            {
                var seedTrends = new List<HeadcountTrendRecord>
                {
                    new HeadcountTrendRecord { Id = "TRN-1", MonthYear = "Jan 2026", TotalEmployees = 1320, NewHires = 35, Departures = 12 },
                    new HeadcountTrendRecord { Id = "TRN-2", MonthYear = "Feb 2026", TotalEmployees = 1348, NewHires = 40, Departures = 12 },
                    new HeadcountTrendRecord { Id = "TRN-3", MonthYear = "Mar 2026", TotalEmployees = 1375, NewHires = 38, Departures = 11 },
                    new HeadcountTrendRecord { Id = "TRN-4", MonthYear = "Apr 2026", TotalEmployees = 1395, NewHires = 30, Departures = 10 },
                    new HeadcountTrendRecord { Id = "TRN-5", MonthYear = "May 2026", TotalEmployees = 1412, NewHires = 25, Departures = 8 },
                    new HeadcountTrendRecord { Id = "TRN-6", MonthYear = "Jun 2026", TotalEmployees = 1428, NewHires = 24, Departures = 8 }
                };
                await _dbContext.Set<HeadcountTrendRecord>().AddRangeAsync(seedTrends);
                await _dbContext.SaveChangesAsync();
                return seedTrends;
            }
            return trends;
        }

        public async Task<IEnumerable<DepartmentDistributionRecord>> GetDepartmentDistributionsAsync()
        {
            var depts = await _dbContext.Set<DepartmentDistributionRecord>().ToListAsync();
            if (!depts.Any())
            {
                var seedDepts = new List<DepartmentDistributionRecord>
                {
                    new DepartmentDistributionRecord { Id = "DPT-1", DepartmentName = "Engineering & AI", EmployeeCount = 680, BudgetUtilization = "94.5%" },
                    new DepartmentDistributionRecord { Id = "DPT-2", DepartmentName = "Product & Design", EmployeeCount = 185, BudgetUtilization = "91.2%" },
                    new DepartmentDistributionRecord { Id = "DPT-3", DepartmentName = "Sales & Customer Success", EmployeeCount = 340, BudgetUtilization = "96.8%" },
                    new DepartmentDistributionRecord { Id = "DPT-4", DepartmentName = "Operations & HR", EmployeeCount = 223, BudgetUtilization = "88.4%" }
                };
                await _dbContext.Set<DepartmentDistributionRecord>().AddRangeAsync(seedDepts);
                await _dbContext.SaveChangesAsync();
                return seedDepts;
            }
            return depts;
        }
    }
}
