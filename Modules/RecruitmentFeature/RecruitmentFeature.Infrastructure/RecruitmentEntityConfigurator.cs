using HRMS.Core.Postgres.Interfaces;
using Microsoft.EntityFrameworkCore;
using RecruitmentFeature.Domain;

namespace RecruitmentFeature.Infrastructure
{
    public class RecruitmentEntityConfigurator : IPostgresEntityConfigurator
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JobRequisitionRecord>(builder =>
            {
                builder.ToTable("JobRequisitions", "recruitment");
                builder.HasKey(x => x.Id);
                builder.Property(x => x.Title).IsRequired();
                builder.Property(x => x.Department).IsRequired();
            });

            modelBuilder.Entity<CandidateApplicationRecord>(builder =>
            {
                builder.ToTable("CandidateApplications", "recruitment");
                builder.HasKey(x => x.Id);
                builder.Property(x => x.JobId).IsRequired();
                builder.Property(x => x.ApplicantName).IsRequired();
                builder.Property(x => x.ApplicantEmail).IsRequired();
            });
        }
    }
}
