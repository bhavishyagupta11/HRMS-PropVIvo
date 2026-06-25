using ContributionsFeature.Application.DTO;
using ContributionsFeature.Application.Repository;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContributionsFeature.Application.Queries
{
    public class GetAvailableItemsQuery : IRequest<IEnumerable<ContributionItemDto>>
    {
    }

    public class GetAvailableItemsQueryHandler : IRequestHandler<GetAvailableItemsQuery, IEnumerable<ContributionItemDto>>
    {
        private readonly IContributionRepository _repository;

        public GetAvailableItemsQueryHandler(IContributionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ContributionItemDto>> Handle(GetAvailableItemsQuery request, CancellationToken cancellationToken)
        {
            var records = await _repository.GetAvailableItemsAsync();
            return records.Select(x => new ContributionItemDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Category = x.Category,
                SuggestedPoints = x.SuggestedPoints,
                Status = x.Status,
                ClaimedByEmployee = x.ClaimedByEmployee
            });
        }
    }
}
