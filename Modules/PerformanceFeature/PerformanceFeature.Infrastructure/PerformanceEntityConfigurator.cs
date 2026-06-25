using HRMS.Core.Postgres.Interfaces;
using Microsoft.EntityFrameworkCore;
using PerformanceFeature.Domain;

namespace PerformanceFeature.Infrastructure
{
    public class PerformanceEntityConfigurator : IPostgresEntityConfigurator
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GoalRecord>(builder =>
            {
                builder.ToTable("Goals", "performance");
                builder.HasKey(x => x.Id);
                builder.Property(x => x.UserId).IsRequired();
                builder.Property(x => x.Title).IsRequired();
                builder.Property(x => x.Description).IsRequired();
                builder.Property(x => x.Status).IsRequired();
            });

            modelBuilder.Entity<PerformanceReviewRecord>(builder =>
            {
                builder.ToTable("PerformanceReviews", "performance");
                builder.HasKey(x => x.Id);
                builder.Property(x => x.UserId).IsRequired();
                builder.Property(x => x.ReviewCycle).IsRequired();
                builder.Property(x => x.ReviewerUserId).IsRequired();
            });
        }
    }
}
