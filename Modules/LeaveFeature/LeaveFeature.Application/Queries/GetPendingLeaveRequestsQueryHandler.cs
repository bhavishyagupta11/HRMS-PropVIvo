using LeaveFeature.Application.DTO;
using LeaveFeature.Application.Repository;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeaveFeature.Application.Queries
{
    public class GetPendingLeaveRequestsQuery : IRequest<IEnumerable<LeaveRequestDto>> { }

    public class GetPendingLeaveRequestsQueryHandler : IRequestHandler<GetPendingLeaveRequestsQuery, IEnumerable<LeaveRequestDto>>
    {
        private readonly ILeaveRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetPendingLeaveRequestsQueryHandler(ILeaveRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<LeaveRequestDto>> Handle(GetPendingLeaveRequestsQuery request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                ?? user?.Claims?.FirstOrDefault(c => c.Type == "sub")?.Value
                ?? throw new UnauthorizedAccessException("User not found in token");

            var role = user?.Claims?.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value 
                ?? user?.Claims?.FirstOrDefault(c => c.Type == "role")?.Value 
                ?? string.Empty;

            var requests = await _repository.GetPendingLeaveRequestsAsync();

            // Filter strictly by approval role
            if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                // Admin sees all
            }
            else if (role.Equals("Manager", StringComparison.OrdinalIgnoreCase))
            {
                requests = requests.Where(r => r.CurrentApprovalLevel == "Manager");
            }
            else if (role.Equals("ReportingManager", StringComparison.OrdinalIgnoreCase))
            {
                requests = requests.Where(r => r.CurrentApprovalLevel == "ReportingManager");
            }
            else if (role.Equals("HR", StringComparison.OrdinalIgnoreCase))
            {
                requests = requests.Where(r => r.CurrentApprovalLevel == "HR");
            }
            else
            {
                return Enumerable.Empty<LeaveRequestDto>();
            }

            return requests.Select(r => new LeaveRequestDto
            {
                Id = r.Id,
                UserId = r.UserId,
                EmployeeName = "Team Member (" + r.UserId.Substring(0, Math.Min(4, r.UserId.Length)) + ")",
                LeaveType = r.LeaveType,
                StartDate = r.StartDate,
                EndDate = r.EndDate,
                TotalDays = r.TotalDays,
                Reason = r.Reason,
                Status = r.Status,
                ApproverId = r.ApproverId,
                ApproverComments = r.ApproverComments,
                CurrentApprovalLevel = r.CurrentApprovalLevel,
                Approvals = r.Approvals.Select(a => new LeaveApprovalDto
                {
                    Id = a.Id,
                    LeaveRequestId = a.LeaveRequestId,
                    Level = a.Level,
                    Status = a.Status,
                    ApproverId = a.ApproverId,
                    Comment = a.Comment,
                    ActionedAt = a.ActionedAt
                }).ToList()
            });
        }
    }
}
