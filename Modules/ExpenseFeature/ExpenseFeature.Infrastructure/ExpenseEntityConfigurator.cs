using ExpenseFeature.Domain;
using HRMS.Core.Postgres.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExpenseFeature.Infrastructure
{
    public class ExpenseEntityConfigurator : IPostgresEntityConfigurator
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReimbursementRecord>(builder =>
            {
                builder.ToTable("ReimbursementRecords", "expenses");
                builder.HasKey(x => x.Id);
                builder.Property(x => x.UserId).IsRequired();
                builder.Property(x => x.Category).IsRequired();
                builder.Property(x => x.Amount).IsRequired();
                builder.Property(x => x.Currency).IsRequired();
                builder.Property(x => x.Description).IsRequired();
            });
        }
    }
}
