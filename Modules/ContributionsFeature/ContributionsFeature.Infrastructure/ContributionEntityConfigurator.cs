using ContributionsFeature.Domain;
using HRMS.Core.Postgres.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ContributionsFeature.Infrastructure
{
    public class ContributionEntityConfigurator : IPostgresEntityConfigurator
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ValueContribution>(builder =>
            {
                builder.ToTable("ValueContributions", "contribution");
                builder.HasKey(x => x.Id);
                builder.Property(x => x.Title).IsRequired();
            });

            modelBuilder.Entity<ContributionItem>(builder =>
            {
                builder.ToTable("ContributionItems", "contribution");
                builder.HasKey(x => x.Id);
                builder.Property(x => x.Title).IsRequired();
            });

            modelBuilder.Entity<ContributionLeaderboard>(builder =>
            {
                builder.ToTable("ContributionLeaderboards", "contribution");
                builder.HasKey(x => x.Id);
                builder.Property(x => x.EmployeeName).IsRequired();
            });
        }
    }
}
