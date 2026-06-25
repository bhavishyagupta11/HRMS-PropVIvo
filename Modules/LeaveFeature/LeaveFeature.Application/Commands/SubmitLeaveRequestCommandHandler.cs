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
    public class SubmitLeaveRequestCommand : IRequest<LeaveRequestDto>
    {
        public SubmitLeaveRequestDto Payload { get; set; } = new();
    }

    public class SubmitLeaveRequestCommandHandler : IRequestHandler<SubmitLeaveRequestCommand, LeaveRequestDto>
    {
        private readonly ILeaveRepository _leaveRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SubmitLeaveRequestCommandHandler(ILeaveRepository leaveRepository, IHttpContextAccessor httpContextAccessor)
        {
            _leaveRepository = leaveRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<LeaveRequestDto> Handle(SubmitLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                ?? user?.Claims?.FirstOrDefault(c => c.Type == "sub")?.Value
                ?? throw new UnauthorizedAccessException("User not found in token");

            var payload = request.Payload;

            // Initialize balances if they don't exist
            var balances = await _leaveRepository.GetLeaveBalancesAsync(userId);
            if (!balances.Any())
            {
                var defaultTypes = new[]
                {
                    new { Name = "Casual Leave", Allowed = 12 },
                    new { Name = "Sick Leave", Allowed = 10 },
                    new { Name = "Personal Leave", Allowed = 5 },
                    new { Name = "Maternity Leave", Allowed = 180 },
                    new { Name = "Paternity Leave", Allowed = 15 },
                    new { Name = "Leave Without Pay", Allowed = 30 },
                    new { Name = "Comp-off", Allowed = 3 }
                };

                foreach (var dt in defaultTypes)
                {
                    await _leaveRepository.CreateLeaveBalanceAsync(new LeaveBalance
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = userId,
                        LeaveType = dt.Name,
                        TotalAllowed = dt.Allowed,
                        Available = dt.Allowed,
                        Used = 0,
                        Pending = 0,
                        CarriedForward = 0,
                        Encashed = 0
                    });
                }
            }

            var balance = await _leaveRepository.GetLeaveBalanceAsync(userId, payload.LeaveType);
            if (balance == null)
            {
                throw new InvalidOperationException($"Leave balance of type {payload.LeaveType} not found.");
            }

            if (balance.Available < payload.TotalDays)
            {
                throw new InvalidOperationException("Insufficient leave balance.");
            }

            // Block the balance
            balance.Available -= payload.TotalDays;
            balance.Pending += payload.TotalDays;
            await _leaveRepository.UpdateLeaveBalanceAsync(balance);

            var leaveRequest = new LeaveRequest
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                LeaveType = payload.LeaveType,
                StartDate = payload.StartDate,
                EndDate = payload.EndDate,
                TotalDays = payload.TotalDays,
                Reason = payload.Reason,
                Status = "Pending",
                CurrentApprovalLevel = "Manager"
            };

            leaveRequest.Approvals = new List<LeaveApproval>
            {
                new LeaveApproval { Id = Guid.NewGuid().ToString(), LeaveRequestId = leaveRequest.Id, Level = "Manager", Status = "Pending" },
                new LeaveApproval { Id = Guid.NewGuid().ToString(), LeaveRequestId = leaveRequest.Id, Level = "ReportingManager", Status = "Pending" },
                new LeaveApproval { Id = Guid.NewGuid().ToString(), LeaveRequestId = leaveRequest.Id, Level = "HR", Status = "Pending" }
            };

            await _leaveRepository.CreateLeaveRequestAsync(leaveRequest);

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
