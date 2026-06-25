using CopilotFeature.Application.DTO;
using CopilotFeature.Application.Queries;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CopilotFeature.GraphQL
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class CopilotQuery
    {
        [Authorize]
        public async Task<IEnumerable<CopilotInteractionRecordDto>> GetInteractionHistory([Service] IMediator mediator)
        {
            return await mediator.Send(new GetInteractionHistoryQuery());
        }
    }
}
