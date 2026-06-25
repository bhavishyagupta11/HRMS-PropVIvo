using CopilotFeature.Application.Repository;
using CopilotFeature.Domain;
using HRMS.Core.Postgres.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CopilotFeature.Infrastructure.Repositories
{
    public class CopilotRepository : ICopilotRepository
    {
        private readonly PostgresDbContext _dbContext;

        public CopilotRepository(PostgresDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<CopilotInteractionRecord>> GetInteractionHistoryAsync()
        {
            var history = await _dbContext.Set<CopilotInteractionRecord>().OrderByDescending(x => x.InteractionDate).ToListAsync();
            if (!history.Any())
            {
                var seedHistory = new List<CopilotInteractionRecord>
                {
                    new CopilotInteractionRecord { Id = "COP-101", PromptText = "What is our annual leave allowance for 2026?", ResponseText = "According to our 2026 Employee Handbook (Section 4.2), full-time employees are entitled to 20 days of paid annual leave, 10 days of sick leave, and standard federal holidays. You can verify your live balance or submit a request directly in the Leave Management tab.", Category = "Leave & Time Off", ConfidenceScore = 0.98, InteractionDate = DateTime.UtcNow.AddHours(-3) },
                    new CopilotInteractionRecord { Id = "COP-102", PromptText = "What is the per diem and receipt policy for client dining?", ResponseText = "Our Global Travel & Expense Policy permits full reimbursement for client dining, domestic flights, and accommodations. Receipts are required for all transactions exceeding $25. Standard approval turnaround is 3-5 business days upon submission in the Expenses tab.", Category = "Expenses & Policy", ConfidenceScore = 0.96, InteractionDate = DateTime.UtcNow.AddDays(-1) }
                };
                await _dbContext.Set<CopilotInteractionRecord>().AddRangeAsync(seedHistory);
                await _dbContext.SaveChangesAsync();
                return seedHistory.OrderByDescending(x => x.InteractionDate);
            }
            return history;
        }

        public async Task SaveInteractionAsync(CopilotInteractionRecord record)
        {
            await _dbContext.Set<CopilotInteractionRecord>().AddAsync(record);
            await _dbContext.SaveChangesAsync();
        }
    }
}
