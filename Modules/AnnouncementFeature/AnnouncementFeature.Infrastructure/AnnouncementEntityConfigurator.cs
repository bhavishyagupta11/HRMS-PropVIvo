using AnnouncementFeature.Domain;
using HRMS.Core.Postgres.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnnouncementFeature.Infrastructure
{
    public class AnnouncementEntityConfigurator : IPostgresEntityConfigurator
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnnouncementRecord>(builder =>
            {
                builder.ToTable("Announcements", "announcement");
                builder.HasKey(x => x.Id);
                builder.Property(x => x.Title).IsRequired();
                builder.Property(x => x.Content).IsRequired();
                builder.Property(x => x.AuthorName).IsRequired();
            });
        }
    }
}
