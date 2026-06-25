using HRMS.Core.Postgres.Interfaces;
using LeaveFeature.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveFeature.Infrastructure
{
    public class LeaveEntityConfigurator : IPostgresEntityConfigurator
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LeaveBalance>(ConfigureLeaveBalance);
            modelBuilder.Entity<LeaveRequest>(ConfigureLeaveRequest);
            modelBuilder.Entity<LeaveApproval>(ConfigureLeaveApproval);
        }

        private void ConfigureLeaveBalance(EntityTypeBuilder<LeaveBalance> builder)
        {
            builder.ToTable("LeaveBalances");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasMaxLength(128);
            builder.Property(x => x.UserId).IsRequired().HasMaxLength(128);
            builder.HasIndex(x => x.UserId);
        }

        private void ConfigureLeaveRequest(EntityTypeBuilder<LeaveRequest> builder)
        {
            builder.ToTable("LeaveRequests");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasMaxLength(128);
            builder.Property(x => x.UserId).IsRequired().HasMaxLength(128);
            builder.HasIndex(x => x.UserId);
            builder.Property(x => x.CurrentApprovalLevel).IsRequired().HasMaxLength(64).HasDefaultValue("Manager");

            builder.HasMany(x => x.Approvals)
                   .WithOne()
                   .HasForeignKey(x => x.LeaveRequestId)
                   .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureLeaveApproval(EntityTypeBuilder<LeaveApproval> builder)
        {
            builder.ToTable("LeaveApprovals");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasMaxLength(128);
            builder.Property(x => x.LeaveRequestId).IsRequired().HasMaxLength(128);
            builder.Property(x => x.Level).IsRequired().HasMaxLength(64);
            builder.Property(x => x.Status).IsRequired().HasMaxLength(64);
            builder.Property(x => x.ApproverId).HasMaxLength(128);
            builder.Property(x => x.Comment).HasMaxLength(500);
        }
    }
}
