using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using RecognitionFeature.Application.DTO;
using RecognitionFeature.Application.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecognitionFeature.GraphQL
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class RecognitionQuery
    {
        [Authorize]
        public async Task<IEnumerable<RecognitionRecordDto>> GetRecentRecognitions([Service] IMediator mediator)
        {
            return await mediator.Send(new GetRecentRecognitionsQuery());
        }
    }
}
