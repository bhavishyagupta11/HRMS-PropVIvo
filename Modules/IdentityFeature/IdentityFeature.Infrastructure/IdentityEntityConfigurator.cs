using HRMS.Core.Postgres.Interfaces;
using IdentityFeature.Domain;
using Microsoft.EntityFrameworkCore;

namespace IdentityFeature.Infrastructure
{
    public class IdentityEntityConfigurator : IPostgresEntityConfigurator
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasMaxLength(128);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(128);
                entity.Property(e => e.DocumentType).IsRequired().HasMaxLength(128);
                entity.HasIndex(e => e.Name).IsUnique();
                entity.OwnsOne(e => e.UserContext, uc =>
                {
                    uc.Property(p => p.CreatedByUserId).IsRequired();
                });
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasMaxLength(128);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
                entity.Property(e => e.DocumentType).IsRequired().HasMaxLength(128);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasOne(e => e.Role)
                      .WithMany()
                      .HasForeignKey(e => e.RoleId)
                      .IsRequired();
                entity.OwnsOne(e => e.UserContext, uc =>
                {
                    uc.Property(p => p.CreatedByUserId).IsRequired();
                });
            });
        }
    }
}
