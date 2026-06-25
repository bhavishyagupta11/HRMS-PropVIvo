using AnnouncementFeature.Application.Repository;
using AnnouncementFeature.Domain;
using HRMS.Core.Postgres.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnnouncementFeature.Infrastructure.Repositories
{
    public class AnnouncementRepository : IAnnouncementRepository
    {
        private readonly PostgresDbContext _dbContext;

        public AnnouncementRepository(PostgresDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<AnnouncementRecord>> GetActiveAnnouncementsAsync()
        {
            var announcements = await _dbContext.Set<AnnouncementRecord>().OrderByDescending(x => x.PublishedDate).ToListAsync();
            if (!announcements.Any())
            {
                var seedAnnouncements = new List<AnnouncementRecord>
                {
                    new AnnouncementRecord { Id = "ANN-401", Title = "Global Company Townhall Q3", Content = "Join us this Friday for our Q3 company updates, product roadmap presentations, and live Q&A with executive leadership.", AuthorName = "CEO Office", Priority = "High", TargetAudience = "All Employees", PublishedDate = DateTime.UtcNow.AddDays(-1) },
                    new AnnouncementRecord { Id = "ANN-402", Title = "Scheduled Azure Cloud Maintenance", Content = "Engineering teams please note: Core cloud infrastructure will undergo scheduled maintenance this Saturday between 02:00 AM and 04:00 AM UTC.", AuthorName = "Cloud Operations", Priority = "Medium", TargetAudience = "Engineering", PublishedDate = DateTime.UtcNow.AddDays(-2) },
                    new AnnouncementRecord { Id = "ANN-403", Title = "New AI Compliance & Copilot Rollout", Content = "We are officially deploying our fully compliant AI Copilot toolset across all enterprise knowledge bases starting next Monday.", AuthorName = "AI Steering Committee", Priority = "Urgent", TargetAudience = "All Employees", PublishedDate = DateTime.UtcNow.AddHours(-6) }
                };
                await _dbContext.Set<AnnouncementRecord>().AddRangeAsync(seedAnnouncements);
                await _dbContext.SaveChangesAsync();
                return seedAnnouncements.OrderByDescending(x => x.PublishedDate);
            }
            return announcements;
        }

        public async Task PublishAnnouncementAsync(AnnouncementRecord record)
        {
            await _dbContext.Set<AnnouncementRecord>().AddAsync(record);
            await _dbContext.SaveChangesAsync();
        }
    }
}
