using AnalyticsFeature.Domain;
using HRMS.Core.Postgres.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnalyticsFeature.Infrastructure
{
    public class AnalyticsEntityConfigurator : IPostgresEntityConfigurator
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnalyticsMetricRecord>(builder =>
            {
                builder.ToTable("AnalyticsMetrics", "analytics");
                builder.HasKey(x => x.Id);
                builder.Property(x => x.MetricName).IsRequired();
            });

            modelBuilder.Entity<HeadcountTrendRecord>(builder =>
            {
                builder.ToTable("HeadcountTrends", "analytics");
                builder.HasKey(x => x.Id);
                builder.Property(x => x.MonthYear).IsRequired();
            });

            modelBuilder.Entity<DepartmentDistributionRecord>(builder =>
            {
                builder.ToTable("DepartmentDistributions", "analytics");
                builder.HasKey(x => x.Id);
                builder.Property(x => x.DepartmentName).IsRequired();
            });
        }
    }
}
