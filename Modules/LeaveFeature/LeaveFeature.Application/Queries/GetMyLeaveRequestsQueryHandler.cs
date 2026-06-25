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
    public class GetMyLeaveRequestsQuery : IRequest<IEnumerable<LeaveRequestDto>> { }

    public class GetMyLeaveRequestsQueryHandler : IRequestHandler<GetMyLeaveRequestsQuery, IEnumerable<LeaveRequestDto>>
    {
        private readonly ILeaveRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMyLeaveRequestsQueryHandler(ILeaveRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<LeaveRequestDto>> Handle(GetMyLeaveRequestsQuery request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                ?? user?.Claims?.FirstOrDefault(c => c.Type == "sub")?.Value
                ?? throw new UnauthorizedAccessException("User not found in token");

            var requests = await _repository.GetLeaveRequestsAsync(userId);
            
            return requests.Select(r => new LeaveRequestDto
            {
                Id = r.Id,
                UserId = r.UserId,
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
