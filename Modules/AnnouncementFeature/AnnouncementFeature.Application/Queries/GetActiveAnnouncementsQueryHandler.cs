using AnnouncementFeature.Application.DTO;
using AnnouncementFeature.Application.Repository;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AnnouncementFeature.Application.Queries
{
    public class GetActiveAnnouncementsQuery : IRequest<IEnumerable<AnnouncementRecordDto>>
    {
    }

    public class GetActiveAnnouncementsQueryHandler : IRequestHandler<GetActiveAnnouncementsQuery, IEnumerable<AnnouncementRecordDto>>
    {
        private readonly IAnnouncementRepository _repository;

        public GetActiveAnnouncementsQueryHandler(IAnnouncementRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<AnnouncementRecordDto>> Handle(GetActiveAnnouncementsQuery request, CancellationToken cancellationToken)
        {
            var records = await _repository.GetActiveAnnouncementsAsync();
            return records.Select(x => new AnnouncementRecordDto
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,
                AuthorName = x.AuthorName,
                Priority = x.Priority,
                TargetAudience = x.TargetAudience,
                PublishedDate = x.PublishedDate
            });
        }
    }
}
