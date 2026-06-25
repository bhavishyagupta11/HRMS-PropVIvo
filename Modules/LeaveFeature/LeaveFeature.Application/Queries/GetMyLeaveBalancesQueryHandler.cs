using LeaveFeature.Application.DTO;
using LeaveFeature.Application.Repository;
using LeaveFeature.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeaveFeature.Application.Queries
{
    public class GetMyLeaveBalancesQuery : IRequest<IEnumerable<LeaveBalanceDto>> { }

    public class GetMyLeaveBalancesQueryHandler : IRequestHandler<GetMyLeaveBalancesQuery, IEnumerable<LeaveBalanceDto>>
    {
        private readonly ILeaveRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMyLeaveBalancesQueryHandler(ILeaveRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<LeaveBalanceDto>> Handle(GetMyLeaveBalancesQuery request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                ?? user?.Claims?.FirstOrDefault(c => c.Type == "sub")?.Value
                ?? throw new UnauthorizedAccessException("User not found in token");

            var balances = (await _repository.GetLeaveBalancesAsync(userId)).ToList();
            
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
                    var newBalance = new LeaveBalance
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
                    };
                    await _repository.CreateLeaveBalanceAsync(newBalance);
                    balances.Add(newBalance);
                }
            }

            return balances.Select(b => new LeaveBalanceDto
            {
                Id = b.Id,
                UserId = b.UserId,
                LeaveType = b.LeaveType,
                TotalAllowed = b.TotalAllowed,
                Used = b.Used,
                Pending = b.Pending,
                Available = b.Available,
                CarriedForward = b.CarriedForward,
                Encashed = b.Encashed
            });
        }
    }
}
