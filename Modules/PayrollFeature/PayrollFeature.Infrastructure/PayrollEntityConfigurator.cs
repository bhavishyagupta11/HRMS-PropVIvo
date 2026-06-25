using HRMS.Core.Postgres.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayrollFeature.Domain;

namespace PayrollFeature.Infrastructure
{
    public class PayrollRecordConfigurator : IPostgresEntityConfigurator
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PayrollRecord>(builder =>
            {
                builder.ToTable("Payslips", "payroll");
                builder.HasKey(x => x.Id);

                builder.Property(x => x.EarningsJson).HasColumnType("jsonb");
                builder.Property(x => x.DeductionsJson).HasColumnType("jsonb");
                builder.Property(x => x.EmployerContributionsJson).HasColumnType("jsonb");
            });

            modelBuilder.Entity<ComplianceDocument>(builder =>
            {
                builder.ToTable("ComplianceDocuments", "payroll");
                builder.HasKey(x => x.Id);
            });
        }
    }
}
