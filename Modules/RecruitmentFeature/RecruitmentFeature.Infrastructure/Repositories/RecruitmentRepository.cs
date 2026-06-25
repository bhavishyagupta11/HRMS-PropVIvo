using HRMS.Core.Postgres.Data;
using Microsoft.EntityFrameworkCore;
using RecruitmentFeature.Application.Repository;
using RecruitmentFeature.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecruitmentFeature.Infrastructure.Repositories
{
    public class RecruitmentRepository : IRecruitmentRepository
    {
        private readonly PostgresDbContext _dbContext;

        public RecruitmentRepository(PostgresDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<JobRequisitionRecord>> GetOpenJobsAsync()
        {
            var jobs = await _dbContext.Set<JobRequisitionRecord>().ToListAsync();
            if (!jobs.Any())
            {
                var seedJobs = new List<JobRequisitionRecord>
                {
                    new JobRequisitionRecord { Id = "JOB-201", Title = "Senior AI Engineer", Department = "AI Research", Location = "Hybrid / San Francisco", Description = "Lead the next generation of enterprise AI Copilot development.", Requirements = "5+ years in Python, C#, PyTorch, and LLM fine-tuning." },
                    new JobRequisitionRecord { Id = "JOB-202", Title = "Staff Cloud Architect", Department = "Platform Engineering", Location = "Remote / US", Description = "Design highly scalable microservices on Azure and Kubernetes.", Requirements = "8+ years in distributed systems, Azure Kubernetes Service, and .NET Core." }
                };
                await _dbContext.Set<JobRequisitionRecord>().AddRangeAsync(seedJobs);
                await _dbContext.SaveChangesAsync();
                return seedJobs;
            }
            return jobs;
        }

        public async Task<IEnumerable<CandidateApplicationRecord>> GetApplicationsAsync(string email)
        {
            return await _dbContext.Set<CandidateApplicationRecord>()
                .Where(x => x.ApplicantEmail == email)
                .OrderByDescending(x => x.AppliedDate)
                .ToListAsync();
        }

        public async Task ApplyAsync(CandidateApplicationRecord record)
        {
            await _dbContext.Set<CandidateApplicationRecord>().AddAsync(record);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<CandidateApplicationRecord?> GetApplicationByIdAsync(string applicationId)
        {
            return await _dbContext.Set<CandidateApplicationRecord>()
                .FirstOrDefaultAsync(x => x.Id == applicationId);
        }

        public async Task UpdateApplicationAsync(CandidateApplicationRecord record)
        {
            _dbContext.Set<CandidateApplicationRecord>().Update(record);
            await _dbContext.SaveChangesAsync();
        }
    }
}
