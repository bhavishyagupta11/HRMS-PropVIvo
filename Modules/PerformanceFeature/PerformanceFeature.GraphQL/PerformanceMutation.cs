using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using PerformanceFeature.Application.Commands;
using PerformanceFeature.Application.DTO;
using System.Threading.Tasks;

namespace PerformanceFeature.GraphQL
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class PerformanceMutation
    {
        [Authorize]
        public async Task<GoalRecordDto> CreateGoal(CreateGoalCommand command, [Service] IMediator mediator)
        {
            return await mediator.Send(command);
        }

        [Authorize]
        public async Task<PerformanceReviewRecordDto> SubmitReview(SubmitReviewCommand command, [Service] IMediator mediator)
        {
            return await mediator.Send(command);
        }
    }
}
