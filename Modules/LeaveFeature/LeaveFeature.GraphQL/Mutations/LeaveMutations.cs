using HotChocolate.Authorization;
using HotChocolate.Types;
using LeaveFeature.Application.Commands;
using LeaveFeature.Application.DTO;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LeaveFeature.GraphQL.Mutations
{
    [ExtendObjectType("Mutation")]
    public class LeaveMutations
    {
        [Authorize]
        public async Task<LeaveRequestDto> SubmitLeaveRequestAsync(
            SubmitLeaveRequestDto input,
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            var command = new SubmitLeaveRequestCommand { Payload = input };
            return await mediator.Send(command, cancellationToken);
        }

        [Authorize(Roles = new[] { "Admin", "Manager", "HR", "ReportingManager" })]
        public async Task<LeaveRequestDto> ProcessLeaveRequestAsync(
            ProcessLeaveRequestDto input,
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            var command = new ProcessLeaveRequestCommand { Payload = input };
            return await mediator.Send(command, cancellationToken);
        }

        [Authorize]
        public async Task<LeaveRequestDto> CancelLeaveRequestAsync(
            string requestId,
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            var command = new CancelLeaveRequestCommand { RequestId = requestId };
            return await mediator.Send(command, cancellationToken);
        }
    }
}
