using ExpenseFeature.Application.Repository;
using ExpenseFeature.Domain;
using HRMS.Core.Postgres.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExpenseFeature.Infrastructure.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly PostgresDbContext _context;

        public ExpenseRepository(PostgresDbContext context)
        {
            _context = context;
        }

        public async Task<ReimbursementRecord?> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await _context.Set<ReimbursementRecord>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<ReimbursementRecord>> GetByUserIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await _context.Set<ReimbursementRecord>()
                .Where(x => x.UserId == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ReimbursementRecord>> GetPendingExpensesAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<ReimbursementRecord>()
                .Where(x => x.Status == "Submitted" || x.Status == "Pending-Approval")
                .ToListAsync(cancellationToken);
        }

        public async Task<ReimbursementRecord> AddAsync(ReimbursementRecord record, CancellationToken cancellationToken)
        {
            await _context.Set<ReimbursementRecord>().AddAsync(record, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return record;
        }

        public async Task<ReimbursementRecord> UpdateAsync(ReimbursementRecord record, CancellationToken cancellationToken)
        {
            _context.Set<ReimbursementRecord>().Update(record);
            await _context.SaveChangesAsync(cancellationToken);
            return record;
        }
    }
}
