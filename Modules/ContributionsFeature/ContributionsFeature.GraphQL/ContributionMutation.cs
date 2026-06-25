using ContributionsFeature.Application.Commands;
using ContributionsFeature.Application.DTO;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using System.Threading.Tasks;

namespace ContributionsFeature.GraphQL
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class ContributionMutation
    {
        [Authorize]
        public async Task<ContributionItemDto> ClaimContributionItem(ClaimContributionItemCommand command, [Service] IMediator mediator)
        {
            return await mediator.Send(command);
        }

        [Authorize]
        public async Task<ValueContributionDto> ApproveContribution(ApproveContributionCommand command, [Service] IMediator mediator)
        {
            return await mediator.Send(command);
        }

        [Authorize]
        public async Task<ValueContributionDto> SubmitContribution(SubmitContributionCommand command, [Service] IMediator mediator)
        {
            return await mediator.Send(command);
        }
    }
}
