using HRMS.Core.Postgres.Data;
using LeaveFeature.Application.Repository;
using LeaveFeature.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveFeature.Infrastructure.Repositories
{
    public class LeaveRepository : ILeaveRepository
    {
        private readonly PostgresDbContext _context;

        public LeaveRepository(PostgresDbContext context)
        {
            _context = context;
        }

        public async Task<LeaveBalance?> GetLeaveBalanceAsync(string userId, string leaveType)
        {
            return await _context.Set<LeaveBalance>()
                .FirstOrDefaultAsync(b => b.UserId == userId && b.LeaveType == leaveType);
        }

        public async Task<IEnumerable<LeaveBalance>> GetLeaveBalancesAsync(string userId)
        {
            return await _context.Set<LeaveBalance>()
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        public async Task UpdateLeaveBalanceAsync(LeaveBalance balance)
        {
            _context.Set<LeaveBalance>().Update(balance);
            await _context.SaveChangesAsync();
        }

        public async Task CreateLeaveBalanceAsync(LeaveBalance balance)
        {
            _context.Set<LeaveBalance>().Add(balance);
            await _context.SaveChangesAsync();
        }

        public async Task<LeaveRequest?> GetLeaveRequestAsync(string id)
        {
            return await _context.Set<LeaveRequest>()
                .Include(r => r.Approvals)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<LeaveRequest>> GetLeaveRequestsAsync(string userId)
        {
            return await _context.Set<LeaveRequest>()
                .Include(r => r.Approvals)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<LeaveRequest>> GetPendingLeaveRequestsAsync()
        {
            return await _context.Set<LeaveRequest>()
                .Include(r => r.Approvals)
                .Where(r => r.Status == "Pending")
                .OrderBy(r => r.StartDate)
                .ToListAsync();
        }

        public async Task CreateLeaveRequestAsync(LeaveRequest request)
        {
            _context.Set<LeaveRequest>().Add(request);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateLeaveRequestAsync(LeaveRequest request)
        {
            _context.Set<LeaveRequest>().Update(request);
            await _context.SaveChangesAsync();
        }
    }
}
