using LeaveFeature.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeaveFeature.Application.Repository
{
    public interface ILeaveRepository
    {
        Task<LeaveBalance?> GetLeaveBalanceAsync(string userId, string leaveType);
        Task<IEnumerable<LeaveBalance>> GetLeaveBalancesAsync(string userId);
        Task UpdateLeaveBalanceAsync(LeaveBalance balance);
        Task CreateLeaveBalanceAsync(LeaveBalance balance);

        Task<LeaveRequest?> GetLeaveRequestAsync(string id);
        Task<IEnumerable<LeaveRequest>> GetLeaveRequestsAsync(string userId);
        Task<IEnumerable<LeaveRequest>> GetPendingLeaveRequestsAsync();
        Task CreateLeaveRequestAsync(LeaveRequest request);
        Task UpdateLeaveRequestAsync(LeaveRequest request);
    }
}
