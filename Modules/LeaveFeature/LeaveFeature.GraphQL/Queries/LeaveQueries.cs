using HotChocolate.Authorization;
using HotChocolate.Types;
using LeaveFeature.Application.DTO;
using LeaveFeature.Application.Queries;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LeaveFeature.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class LeaveQueries
    {
        [Authorize]
        public async Task<IEnumerable<LeaveBalanceDto>> GetMyLeaveBalancesAsync(
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetMyLeaveBalancesQuery(), cancellationToken);
        }

        [Authorize]
        public async Task<IEnumerable<LeaveRequestDto>> GetMyLeaveRequestsAsync(
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetMyLeaveRequestsQuery(), cancellationToken);
        }

        [Authorize(Roles = new[] { "Admin", "Manager", "ReportingManager", "HR" })]
        public async Task<IEnumerable<LeaveRequestDto>> GetPendingLeaveRequestsAsync(
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetPendingLeaveRequestsQuery(), cancellationToken);
        }
    }
}
