using LeaveFeature.Application.DTO;
using LeaveFeature.Application.Repository;
using LeaveFeature.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeaveFeature.Application.Commands
{
    public class ProcessLeaveRequestCommand : IRequest<LeaveRequestDto>
    {
        public ProcessLeaveRequestDto Payload { get; set; } = new();
    }

    public class ProcessLeaveRequestCommandHandler : IRequestHandler<ProcessLeaveRequestCommand, LeaveRequestDto>
    {
        private readonly ILeaveRepository _leaveRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProcessLeaveRequestCommandHandler(ILeaveRepository leaveRepository, IHttpContextAccessor httpContextAccessor)
        {
            _leaveRepository = leaveRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<LeaveRequestDto> Handle(ProcessLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                ?? user?.Claims?.FirstOrDefault(c => c.Type == "sub")?.Value
                ?? throw new UnauthorizedAccessException("User not found in token");

            var role = user?.Claims?.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value 
                ?? user?.Claims?.FirstOrDefault(c => c.Type == "role")?.Value 
                ?? string.Empty;

            var leaveRequest = await _leaveRepository.GetLeaveRequestAsync(request.Payload.RequestId);
            if (leaveRequest == null) throw new InvalidOperationException("Leave request not found");

            if (leaveRequest.Status != "Pending")
            {
                throw new InvalidOperationException("Leave request is already processed.");
            }

            var currentLevel = leaveRequest.CurrentApprovalLevel;
            bool isAdmin = role.Equals("Admin", StringComparison.OrdinalIgnoreCase);

            if (currentLevel == "Manager")
            {
                if (!role.Equals("Manager", StringComparison.OrdinalIgnoreCase) && !isAdmin)
                    throw new UnauthorizedAccessException("Only Managers can approve/reject at the Manager level.");
            }
            else if (currentLevel == "ReportingManager")
            {
                if (!role.Equals("ReportingManager", StringComparison.OrdinalIgnoreCase) && !isAdmin)
                    throw new UnauthorizedAccessException("Only Reporting Managers can approve/reject at the Reporting Manager level.");
            }
            else if (currentLevel == "HR")
            {
                if (!role.Equals("HR", StringComparison.OrdinalIgnoreCase) && !isAdmin)
                    throw new UnauthorizedAccessException("Only HR can approve/reject at the HR level.");
            }
            else
            {
                throw new InvalidOperationException("Invalid approval stage.");
            }

            var currentApproval = leaveRequest.Approvals.FirstOrDefault(a => a.Level == currentLevel);
            if (currentApproval == null)
            {
                throw new InvalidOperationException($"No pending approval stage found for level: {currentLevel}");
            }

            if (request.Payload.Approve)
            {
                currentApproval.Status = "Approved";
                currentApproval.ApproverId = userId;
                currentApproval.Comment = request.Payload.Comments;
                currentApproval.ActionedAt = DateTime.UtcNow;

                if (currentLevel == "Manager")
                {
                    leaveRequest.CurrentApprovalLevel = "ReportingManager";
                }
                else if (currentLevel == "ReportingManager")
                {
                    leaveRequest.CurrentApprovalLevel = "HR";
                }
                else if (currentLevel == "HR")
                {
                    leaveRequest.CurrentApprovalLevel = "Completed";
                    leaveRequest.Status = "Approved";
                    leaveRequest.ApproverId = userId;
                    leaveRequest.ApproverComments = request.Payload.Comments;

                    var balance = await _leaveRepository.GetLeaveBalanceAsync(leaveRequest.UserId, leaveRequest.LeaveType);
                    if (balance != null)
                    {
                        balance.Pending = Math.Max(0, balance.Pending - leaveRequest.TotalDays);
                        balance.Used += leaveRequest.TotalDays;
                        await _leaveRepository.UpdateLeaveBalanceAsync(balance);
                    }
                }
            }
            else
            {
                currentApproval.Status = "Rejected";
                currentApproval.ApproverId = userId;
                currentApproval.Comment = request.Payload.Comments;
                currentApproval.ActionedAt = DateTime.UtcNow;

                leaveRequest.Status = "Rejected";
                leaveRequest.ApproverId = userId;
                leaveRequest.ApproverComments = request.Payload.Comments;

                var balance = await _leaveRepository.GetLeaveBalanceAsync(leaveRequest.UserId, leaveRequest.LeaveType);
                if (balance != null)
                {
                    balance.Pending = Math.Max(0, balance.Pending - leaveRequest.TotalDays);
                    balance.Available += leaveRequest.TotalDays;
                    await _leaveRepository.UpdateLeaveBalanceAsync(balance);
                }
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
                ApproverId = leaveRequest.ApproverId,
                ApproverComments = leaveRequest.ApproverComments,
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
