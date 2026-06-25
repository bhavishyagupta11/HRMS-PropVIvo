using AnnouncementFeature.Application.DTO;
using AnnouncementFeature.Application.Queries;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnnouncementFeature.GraphQL
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class AnnouncementQuery
    {
        [Authorize]
        public async Task<IEnumerable<AnnouncementRecordDto>> GetActiveAnnouncements([Service] IMediator mediator)
        {
            return await mediator.Send(new GetActiveAnnouncementsQuery());
        }
    }
}
