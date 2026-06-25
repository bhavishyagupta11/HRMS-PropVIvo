using HRMS.Core.Postgres.Data;
using Microsoft.EntityFrameworkCore;
using RecognitionFeature.Application.Repository;
using RecognitionFeature.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecognitionFeature.Infrastructure.Repositories
{
    public class RecognitionRepository : IRecognitionRepository
    {
        private readonly PostgresDbContext _dbContext;

        public RecognitionRepository(PostgresDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<RecognitionRecord>> GetRecentRecognitionsAsync()
        {
            var recs = await _dbContext.Set<RecognitionRecord>().OrderByDescending(x => x.AwardedDate).ToListAsync();
            if (!recs.Any())
            {
                var seedRecs = new List<RecognitionRecord>
                {
                    new RecognitionRecord { Id = "REC-301", GiverName = "Sarah Jenkins", ReceiverName = "Alex Mercer", ReceiverEmail = "alex.m@enterprise.hrms", Category = "Innovation", Message = "Flawless deployment of the Kubernetes ingress controller under tight deadlines!", Points = 100, AwardedDate = System.DateTime.UtcNow.AddDays(-2) },
                    new RecognitionRecord { Id = "REC-302", GiverName = "Dr. Robert Chen", ReceiverName = "Elena Rostova", ReceiverEmail = "elena.r@enterprise.hrms", Category = "Teamwork", Message = "Always stepping up to mentor junior engineers on PyTorch architecture.", Points = 150, AwardedDate = System.DateTime.UtcNow.AddDays(-1) }
                };
                await _dbContext.Set<RecognitionRecord>().AddRangeAsync(seedRecs);
                await _dbContext.SaveChangesAsync();
                return seedRecs;
            }
            return recs;
        }

        public async Task SendRecognitionAsync(RecognitionRecord record)
        {
            await _dbContext.Set<RecognitionRecord>().AddAsync(record);
            await _dbContext.SaveChangesAsync();
        }
    }
}
