using HRMS.Core.Postgres.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OnboardingFeature.Domain;

namespace OnboardingFeature.Infrastructure
{
    public class OnboardingEntityConfigurator : IPostgresEntityConfigurator
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OnboardingEmployee>(entity =>
            {
                entity.ToTable("OnboardingEmployees");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasMaxLength(128);
                entity.Property(e => e.UserId).IsRequired().HasMaxLength(128);
                entity.Property(e => e.Designation).HasMaxLength(256);
                entity.Property(e => e.Department).HasMaxLength(256);
                entity.Property(e => e.ManagerName).HasMaxLength(256);
                entity.Property(e => e.BuddyName).HasMaxLength(256);
                entity.Property(e => e.DocumentType).IsRequired().HasMaxLength(128);
                entity.HasIndex(e => e.UserId);
                entity.OwnsOne(e => e.UserContext, uc =>
                {
                    uc.Property(p => p.CreatedByUserId).IsRequired();
                });
            });

            modelBuilder.Entity<OnboardingTask>(entity =>
            {
                entity.ToTable("OnboardingTasks");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasMaxLength(128);
                entity.Property(e => e.OnboardingEmployeeId).IsRequired().HasMaxLength(128);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Phase).IsRequired().HasMaxLength(64);
                entity.Property(e => e.Priority).IsRequired().HasMaxLength(32);
                entity.Property(e => e.Assignee).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(64);
                entity.Property(e => e.DocumentType).IsRequired().HasMaxLength(128);
                entity.HasIndex(e => e.OnboardingEmployeeId);
                entity.OwnsOne(e => e.UserContext, uc =>
                {
                    uc.Property(p => p.CreatedByUserId).IsRequired();
                });
            });

            modelBuilder.Entity<WelcomeMessage>(entity =>
            {
                entity.ToTable("WelcomeMessages");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasMaxLength(128);
                entity.Property(e => e.OnboardingEmployeeId).IsRequired().HasMaxLength(128);
                entity.Property(e => e.SenderName).IsRequired().HasMaxLength(256);
                entity.Property(e => e.SenderRole).IsRequired().HasMaxLength(128);
                entity.Property(e => e.Message).IsRequired();
                entity.Property(e => e.DocumentType).IsRequired().HasMaxLength(128);
                entity.HasIndex(e => e.OnboardingEmployeeId);
                entity.OwnsOne(e => e.UserContext, uc =>
                {
                    uc.Property(p => p.CreatedByUserId).IsRequired();
                });
            });

            modelBuilder.Entity<OnboardingTrainingModuleRef>(entity =>
            {
                entity.ToTable("OnboardingTrainingModuleRefs");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasMaxLength(128);
                entity.Property(e => e.OnboardingEmployeeId).IsRequired().HasMaxLength(128);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(256);
                entity.Property(e => e.DocumentType).IsRequired().HasMaxLength(128);
                entity.HasIndex(e => e.OnboardingEmployeeId);
                entity.OwnsOne(e => e.UserContext, uc =>
                {
                    uc.Property(p => p.CreatedByUserId).IsRequired();
                });
            });

            modelBuilder.Entity<RelocationSupport>(entity =>
            {
                entity.ToTable("RelocationSupports");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasMaxLength(128);
                entity.Property(e => e.OnboardingEmployeeId).IsRequired().HasMaxLength(128);
                entity.Property(e => e.RelocationStatus).HasMaxLength(128);
                entity.Property(e => e.VisaStatus).HasMaxLength(128);
                entity.Property(e => e.DocumentType).IsRequired().HasMaxLength(128);
                // SupportTickets stored as JSON column
                entity.Property(e => e.SupportTickets)
                    .HasConversion(
                        v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                        v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new List<string>())
                    .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                        (c1, c2) => (c1 != null && c2 != null && c1.SequenceEqual(c2)) || (c1 == null && c2 == null),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c.ToList()));
                entity.HasIndex(e => e.OnboardingEmployeeId);
                entity.OwnsOne(e => e.UserContext, uc =>
                {
                    uc.Property(p => p.CreatedByUserId).IsRequired();
                });
            });

            modelBuilder.Entity<TeamIntroduction>(entity =>
            {
                entity.ToTable("TeamIntroductions");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasMaxLength(128);
                entity.Property(e => e.OnboardingEmployeeId).IsRequired().HasMaxLength(128);
                entity.Property(e => e.TeamMemberName).IsRequired().HasMaxLength(256);
                entity.Property(e => e.IntroductionStatus).HasMaxLength(64);
                entity.Property(e => e.DocumentType).IsRequired().HasMaxLength(128);
                entity.HasIndex(e => e.OnboardingEmployeeId);
                entity.OwnsOne(e => e.UserContext, uc =>
                {
                    uc.Property(p => p.CreatedByUserId).IsRequired();
                });
            });

            modelBuilder.Entity<OnboardingMilestone>(entity =>
            {
                entity.ToTable("OnboardingMilestones");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasMaxLength(128);
                entity.Property(e => e.OnboardingEmployeeId).IsRequired().HasMaxLength(128);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Type).IsRequired().HasMaxLength(64);
                entity.Property(e => e.DocumentType).IsRequired().HasMaxLength(128);
                entity.HasIndex(e => e.OnboardingEmployeeId);
                entity.OwnsOne(e => e.UserContext, uc =>
                {
                    uc.Property(p => p.CreatedByUserId).IsRequired();
                });
            });
        }
    }
}
