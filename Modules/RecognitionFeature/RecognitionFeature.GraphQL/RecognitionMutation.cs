using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using RecognitionFeature.Application.Commands;
using RecognitionFeature.Application.DTO;
using System.Threading.Tasks;

namespace RecognitionFeature.GraphQL
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class RecognitionMutation
    {
        [Authorize]
        public async Task<RecognitionRecordDto> SendRecognition(SendRecognitionCommand command, [Service] IMediator mediator)
        {
            return await mediator.Send(command);
        }
    }
}
