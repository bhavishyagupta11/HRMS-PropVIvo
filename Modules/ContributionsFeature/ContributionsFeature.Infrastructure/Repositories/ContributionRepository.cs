using ContributionsFeature.Application.Repository;
using ContributionsFeature.Domain;
using HRMS.Core.Postgres.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContributionsFeature.Infrastructure.Repositories
{
    public class ContributionRepository : IContributionRepository
    {
        private readonly PostgresDbContext _dbContext;

        public ContributionRepository(PostgresDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ValueContribution>> GetContributionsAsync()
        {
            var contributions = await _dbContext.Set<ValueContribution>().OrderByDescending(x => x.SubmittedDate).ToListAsync();
            if (!contributions.Any())
            {
                var seedContributions = new List<ValueContribution>
                {
                    new ValueContribution { Id = "VC-101", Title = "Automated Onboarding Document Parsing", Description = "Built an OCR pipeline to extract fields from uploaded compliance documents automatically.", ContributionType = "self-initiated", Category = "innovation", Status = "completed", Points = 250, SuggestedPoints = 250, ImpactLevel = "High", EmployeeName = "Sarah (Employee)", ApproverName = "Michael (Manager)", ApprovalComments = "Excellent innovation, saves 10 hours per week.", SubmittedDate = DateTime.UtcNow.AddDays(-10), ApprovedDate = DateTime.UtcNow.AddDays(-8) },
                    new ValueContribution { Id = "VC-102", Title = "Cloud Resource Cost Optimization", Description = "Identified and decommissioned unused staging environments across Azure subscriptions.", ContributionType = "committed", Category = "cost-saving", Status = "completed", Points = 180, SuggestedPoints = 200, ImpactLevel = "Medium", EmployeeName = "Sarah (Employee)", ApproverName = "Michael (Manager)", ApprovalComments = "Great initiative on cost reduction.", SubmittedDate = DateTime.UtcNow.AddDays(-5), ApprovedDate = DateTime.UtcNow.AddDays(-3) },
                    new ValueContribution { Id = "VC-103", Title = "Core Architecture Peer Review Workflow", Description = "Streamlined pull request approval templates for faster turnaround.", ContributionType = "assigned", Category = "process-improvement", Status = "under-review", Points = 0, SuggestedPoints = 150, ImpactLevel = "Medium", EmployeeName = "Sarah (Employee)", ApproverName = "", ApprovalComments = "", SubmittedDate = DateTime.UtcNow.AddDays(-1), ApprovedDate = null }
                };
                await _dbContext.Set<ValueContribution>().AddRangeAsync(seedContributions);
                await _dbContext.SaveChangesAsync();
                return seedContributions.OrderByDescending(x => x.SubmittedDate);
            }
            return contributions;
        }

        public async Task<IEnumerable<ContributionItem>> GetAvailableItemsAsync()
        {
            var items = await _dbContext.Set<ContributionItem>().ToListAsync();
            if (!items.Any())
            {
                var seedItems = new List<ContributionItem>
                {
                    new ContributionItem { Id = "CI-201", Title = "Create Automated QA Integration Tests", Description = "Develop end-to-end integration tests for the payroll calculation module.", Category = "quality", SuggestedPoints = 300, Status = "Available", ClaimedByEmployee = "" },
                    new ContributionItem { Id = "CI-202", Title = "Conduct Lunch & Learn on AI Copilot", Description = "Host a 45-minute knowledge sharing session on utilizing the HR Copilot for daily workflows.", Category = "team-building", SuggestedPoints = 150, Status = "Available", ClaimedByEmployee = "" },
                    new ContributionItem { Id = "CI-203", Title = "Optimize PostgreSQL Query Performance", Description = "Review and add missing indexes to high-frequency analytics queries.", Category = "process-improvement", SuggestedPoints = 250, Status = "Available", ClaimedByEmployee = "" }
                };
                await _dbContext.Set<ContributionItem>().AddRangeAsync(seedItems);
                await _dbContext.SaveChangesAsync();
                return seedItems;
            }
            return items;
        }

        public async Task<IEnumerable<ContributionLeaderboard>> GetLeaderboardAsync()
        {
            var leaderboard = await _dbContext.Set<ContributionLeaderboard>().OrderByDescending(x => x.TotalPoints).ToListAsync();
            if (!leaderboard.Any())
            {
                var seedLeaderboard = new List<ContributionLeaderboard>
                {
                    new ContributionLeaderboard { Id = "LB-301", EmployeeName = "Sarah (Employee)", Department = "Engineering", TotalPoints = 430, Badges = "Gold Champion, Innovation Star", AverageRating = 4.9 },
                    new ContributionLeaderboard { Id = "LB-302", EmployeeName = "Michael (Manager)", Department = "Management", TotalPoints = 390, Badges = "Leadership Guru, Process Champion", AverageRating = 4.8 },
                    new ContributionLeaderboard { Id = "LB-303", EmployeeName = "Alex (New Joiner)", Department = "Engineering", TotalPoints = 120, Badges = "Quick Starter", AverageRating = 4.5 },
                    new ContributionLeaderboard { Id = "LB-304", EmployeeName = "David Kumar", Department = "Operations", TotalPoints = 310, Badges = "Cost Saver, Execution Pro", AverageRating = 4.7 }
                };
                await _dbContext.Set<ContributionLeaderboard>().AddRangeAsync(seedLeaderboard);
                await _dbContext.SaveChangesAsync();
                return seedLeaderboard.OrderByDescending(x => x.TotalPoints);
            }
            return leaderboard;
        }

        public async Task<ContributionItem> ClaimContributionItemAsync(string itemId, string employeeName)
        {
            var item = await _dbContext.Set<ContributionItem>().FirstOrDefaultAsync(x => x.Id == itemId);
            if (item != null)
            {
                item.Status = "Claimed";
                item.ClaimedByEmployee = employeeName;
                _dbContext.Set<ContributionItem>().Update(item);
                await _dbContext.SaveChangesAsync();
                return item;
            }
            throw new Exception($"Contribution item with ID {itemId} not found.");
        }

        public async Task<ValueContribution> ApproveContributionAsync(string contributionId, int finalPoints, string comments, string approverName)
        {
            var contribution = await _dbContext.Set<ValueContribution>().FirstOrDefaultAsync(x => x.Id == contributionId);
            if (contribution != null)
            {
                contribution.Status = "completed";
                contribution.Points = finalPoints;
                contribution.ApprovalComments = comments;
                contribution.ApproverName = approverName;
                contribution.ApprovedDate = DateTime.UtcNow;
                _dbContext.Set<ValueContribution>().Update(contribution);

                // Update leaderboard as well
                var lb = await _dbContext.Set<ContributionLeaderboard>().FirstOrDefaultAsync(x => x.EmployeeName == contribution.EmployeeName);
                if (lb != null)
                {
                    lb.TotalPoints += finalPoints;
                    _dbContext.Set<ContributionLeaderboard>().Update(lb);
                }

                await _dbContext.SaveChangesAsync();
                return contribution;
            }
            throw new Exception($"Value contribution with ID {contributionId} not found.");
        }

        public async Task<ValueContribution> SubmitContributionAsync(ValueContribution contribution)
        {
            await _dbContext.Set<ValueContribution>().AddAsync(contribution);
            await _dbContext.SaveChangesAsync();
            return contribution;
        }
    }
}
