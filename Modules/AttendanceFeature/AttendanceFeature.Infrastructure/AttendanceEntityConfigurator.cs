using HRMS.Core.Postgres.Interfaces;
using Microsoft.EntityFrameworkCore;
using AttendanceFeature.Domain;

namespace AttendanceFeature.Infrastructure
{
    public class AttendanceEntityConfigurator : IPostgresEntityConfigurator
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AttendanceRecord>(entity =>
            {
                entity.ToTable("AttendanceRecords");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasMaxLength(128);
                entity.Property(e => e.UserId).IsRequired().HasMaxLength(128);
                entity.Property(e => e.DocumentType).IsRequired().HasMaxLength(128);
                entity.HasIndex(e => e.UserId);
                
                entity.OwnsOne(e => e.UserContext, uc =>
                {
                    uc.Property(p => p.CreatedByUserId).IsRequired();
                });
            });
        }
    }
}
