using ContributionsFeature.Application.DTO;
using ContributionsFeature.Application.Repository;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContributionsFeature.Application.Queries
{
    public class GetLeaderboardQuery : IRequest<IEnumerable<ContributionLeaderboardDto>>
    {
    }

    public class GetLeaderboardQueryHandler : IRequestHandler<GetLeaderboardQuery, IEnumerable<ContributionLeaderboardDto>>
    {
        private readonly IContributionRepository _repository;

        public GetLeaderboardQueryHandler(IContributionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ContributionLeaderboardDto>> Handle(GetLeaderboardQuery request, CancellationToken cancellationToken)
        {
            var records = await _repository.GetLeaderboardAsync();
            return records.Select(x => new ContributionLeaderboardDto
            {
                Id = x.Id,
                EmployeeName = x.EmployeeName,
                Department = x.Department,
                TotalPoints = x.TotalPoints,
                Badges = x.Badges,
                AverageRating = x.AverageRating
            });
        }
    }
}
