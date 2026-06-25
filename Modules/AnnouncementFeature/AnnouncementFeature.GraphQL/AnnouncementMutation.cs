using AnnouncementFeature.Application.Commands;
using AnnouncementFeature.Application.DTO;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using System.Threading.Tasks;

namespace AnnouncementFeature.GraphQL
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class AnnouncementMutation
    {
        [Authorize(Roles = new[] { "Admin", "HR" })]
        public async Task<AnnouncementRecordDto> PublishAnnouncement(PublishAnnouncementCommand command, [Service] IMediator mediator)
        {
            return await mediator.Send(command);
        }
    }
}
