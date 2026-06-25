using ExpenseFeature.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExpenseFeature.Application.Repository
{
    public interface IExpenseRepository
    {
        Task<ReimbursementRecord?> GetByIdAsync(string id, CancellationToken cancellationToken);
        Task<IEnumerable<ReimbursementRecord>> GetByUserIdAsync(string userId, CancellationToken cancellationToken);
        Task<IEnumerable<ReimbursementRecord>> GetPendingExpensesAsync(CancellationToken cancellationToken);
        Task<ReimbursementRecord> AddAsync(ReimbursementRecord record, CancellationToken cancellationToken);
        Task<ReimbursementRecord> UpdateAsync(ReimbursementRecord record, CancellationToken cancellationToken);
    }
}
