using LeaveFeature.Application.DTO;
using LeaveFeature.Application.Repository;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeaveFeature.Application.Commands
{
    public class CancelLeaveRequestCommand : IRequest<LeaveRequestDto>
    {
        public string RequestId { get; set; } = string.Empty;
    }

    public class CancelLeaveRequestCommandHandler : IRequestHandler<CancelLeaveRequestCommand, LeaveRequestDto>
    {
        private readonly ILeaveRepository _leaveRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CancelLeaveRequestCommandHandler(ILeaveRepository leaveRepository, IHttpContextAccessor httpContextAccessor)
        {
            _leaveRepository = leaveRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<LeaveRequestDto> Handle(CancelLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                ?? user?.Claims?.FirstOrDefault(c => c.Type == "sub")?.Value
                ?? throw new UnauthorizedAccessException("User not found in token");

            var leaveRequest = await _leaveRepository.GetLeaveRequestAsync(request.RequestId);
            if (leaveRequest == null) throw new InvalidOperationException("Leave request not found");

            if (leaveRequest.UserId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to cancel other users' leave requests.");
            }

            if (leaveRequest.Status != "Pending")
            {
                throw new InvalidOperationException($"Only pending requests can be cancelled. Current status is: {leaveRequest.Status}");
            }

            leaveRequest.Status = "Cancelled";
            leaveRequest.CurrentApprovalLevel = "Cancelled";

            foreach (var approval in leaveRequest.Approvals.Where(a => a.Status == "Pending"))
            {
                approval.Status = "Cancelled";
                approval.ActionedAt = DateTime.UtcNow;
            }

            var balance = await _leaveRepository.GetLeaveBalanceAsync(leaveRequest.UserId, leaveRequest.LeaveType);
            if (balance != null)
            {
                balance.Pending = Math.Max(0, balance.Pending - leaveRequest.TotalDays);
                balance.Available += leaveRequest.TotalDays;
                await _leaveRepository.UpdateLeaveBalanceAsync(balance);
            }

            await _leaveRepository.UpdateLeaveRequestAsync(leaveRequest);

            return new LeaveRequestDto
            {
                Id = leaveRequest.Id,
                UserId = leaveRequest.UserId,
                LeaveType = leaveRequest.LeaveType,
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                TotalDays = leaveRequest.TotalDays,
                Reason = leaveRequest.Reason,
                Status = leaveRequest.Status,
                CurrentApprovalLevel = leaveRequest.CurrentApprovalLevel,
                Approvals = leaveRequest.Approvals.Select(a => new LeaveApprovalDto
                {
                    Id = a.Id,
                    LeaveRequestId = a.LeaveRequestId,
                    Level = a.Level,
                    Status = a.Status,
                    ApproverId = a.ApproverId,
                    Comment = a.Comment,
                    ActionedAt = a.ActionedAt
                }).ToList()
            };
        }
    }
}
