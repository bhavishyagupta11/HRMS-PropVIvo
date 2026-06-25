using HRMS.Core.Postgres.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainingFeature.Application.Repository;
using TrainingFeature.Domain;

namespace TrainingFeature.Infrastructure.Repositories
{
    public class TrainingRepository : ITrainingRepository
    {
        private readonly PostgresDbContext _dbContext;

        public TrainingRepository(PostgresDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TrainingCourseRecord>> GetAvailableCoursesAsync()
        {
            var courses = await _dbContext.Set<TrainingCourseRecord>().ToListAsync();
            if (!courses.Any())
            {
                // Seed some initial courses if empty
                var seedCourses = new List<TrainingCourseRecord>
                {
                    new TrainingCourseRecord { Id = "CRS-101", Title = "Cloud Native Architecture", Description = "Microservices & Kubernetes masterclass", TrainerName = "Sarah Jenkins", Credits = 40, ScheduleDate = System.DateTime.UtcNow.AddDays(15) },
                    new TrainingCourseRecord { Id = "CRS-102", Title = "Enterprise AI & Copilots", Description = "Integrating generative AI in enterprise solutions", TrainerName = "Dr. Robert Chen", Credits = 50, ScheduleDate = System.DateTime.UtcNow.AddDays(30) }
                };
                await _dbContext.Set<TrainingCourseRecord>().AddRangeAsync(seedCourses);
                await _dbContext.SaveChangesAsync();
                return seedCourses;
            }
            return courses;
        }

        public async Task<IEnumerable<CourseEnrollmentRecord>> GetMyEnrollmentsAsync(string userId)
        {
            return await _dbContext.Set<CourseEnrollmentRecord>()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.EnrollmentDate)
                .ToListAsync();
        }

        public async Task EnrollAsync(CourseEnrollmentRecord record)
        {
            await _dbContext.Set<CourseEnrollmentRecord>().AddAsync(record);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<CourseEnrollmentRecord?> GetEnrollmentAsync(string enrollmentId, string userId)
        {
            return await _dbContext.Set<CourseEnrollmentRecord>()
                .FirstOrDefaultAsync(x => x.Id == enrollmentId && x.UserId == userId);
        }

        public async Task UpdateEnrollmentAsync(CourseEnrollmentRecord record)
        {
            _dbContext.Set<CourseEnrollmentRecord>().Update(record);
            await _dbContext.SaveChangesAsync();
        }
    }
}
