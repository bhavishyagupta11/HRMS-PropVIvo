using ContributionsFeature.Application.DTO;
using ContributionsFeature.Application.Repository;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContributionsFeature.Application.Queries
{
    public class GetContributionsQuery : IRequest<IEnumerable<ValueContributionDto>>
    {
    }

    public class GetContributionsQueryHandler : IRequestHandler<GetContributionsQuery, IEnumerable<ValueContributionDto>>
    {
        private readonly IContributionRepository _repository;

        public GetContributionsQueryHandler(IContributionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ValueContributionDto>> Handle(GetContributionsQuery request, CancellationToken cancellationToken)
        {
            var records = await _repository.GetContributionsAsync();
            return records.Select(x => new ValueContributionDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                ContributionType = x.ContributionType,
                Category = x.Category,
                Status = x.Status,
                Points = x.Points,
                SuggestedPoints = x.SuggestedPoints,
                ImpactLevel = x.ImpactLevel,
                EmployeeName = x.EmployeeName,
                ApproverName = x.ApproverName,
                ApprovalComments = x.ApprovalComments,
                SubmittedDate = x.SubmittedDate,
                ApprovedDate = x.ApprovedDate
            });
        }
    }
}
