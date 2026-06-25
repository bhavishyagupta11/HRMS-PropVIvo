using CopilotFeature.Application.Commands;
using CopilotFeature.Application.DTO;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using System.Threading.Tasks;

namespace CopilotFeature.GraphQL
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class CopilotMutation
    {
        [Authorize]
        public async Task<CopilotInteractionRecordDto> SubmitCopilotPrompt(SubmitCopilotPromptCommand command, [Service] IMediator mediator)
        {
            return await mediator.Send(command);
        }
    }
}
