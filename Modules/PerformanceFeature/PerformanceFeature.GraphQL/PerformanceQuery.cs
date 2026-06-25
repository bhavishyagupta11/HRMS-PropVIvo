using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using PerformanceFeature.Application.DTO;
using PerformanceFeature.Application.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PerformanceFeature.GraphQL
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class PerformanceQuery
    {
        [Authorize]
        public async Task<IEnumerable<GoalRecordDto>> GetMyGoals([Service] IMediator mediator)
        {
            return await mediator.Send(new GetMyGoalsQuery());
        }

        [Authorize]
        public async Task<IEnumerable<PerformanceReviewRecordDto>> GetMyReviews([Service] IMediator mediator)
        {
            return await mediator.Send(new GetMyReviewsQuery());
        }
    }
}
