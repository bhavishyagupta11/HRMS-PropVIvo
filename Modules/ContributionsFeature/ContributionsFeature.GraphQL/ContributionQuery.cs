using ContributionsFeature.Application.DTO;
using ContributionsFeature.Application.Queries;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContributionsFeature.GraphQL
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class ContributionQuery
    {
        [Authorize]
        public async Task<IEnumerable<ValueContributionDto>> GetContributions([Service] IMediator mediator)
        {
            return await mediator.Send(new GetContributionsQuery());
        }

        [Authorize]
        public async Task<IEnumerable<ContributionItemDto>> GetAvailableContributionItems([Service] IMediator mediator)
        {
            return await mediator.Send(new GetAvailableItemsQuery());
        }

        [Authorize]
        public async Task<IEnumerable<ContributionLeaderboardDto>> GetContributionLeaderboard([Service] IMediator mediator)
        {
            return await mediator.Send(new GetLeaderboardQuery());
        }
    }
}
