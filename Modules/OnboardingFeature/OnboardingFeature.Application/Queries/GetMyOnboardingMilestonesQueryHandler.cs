using MediatR;
using OnboardingFeature.Application.DTO;
using OnboardingFeature.Application.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnboardingFeature.Application.Queries
{
    // PSD Key Capabilities: "Onboarding milestones (check-ins, reviews, celebrations) with scheduled dates."
    public class GetMyOnboardingMilestonesQuery : IRequest<List<OnboardingMilestoneDto>>
    {
        public string OnboardingEmployeeId { get; set; } = string.Empty;
    }

    public class GetMyOnboardingMilestonesQueryHandler : IRequestHandler<GetMyOnboardingMilestonesQuery, List<OnboardingMilestoneDto>>
    {
        private readonly IOnboardingRepository _repository;

        public GetMyOnboardingMilestonesQueryHandler(IOnboardingRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<OnboardingMilestoneDto>> Handle(GetMyOnboardingMilestonesQuery request, CancellationToken cancellationToken)
        {
            var milestones = await _repository.GetMilestonesByEmployeeIdAsync(request.OnboardingEmployeeId);

            return milestones.Select(m => new OnboardingMilestoneDto
            {
                Id = m.Id,
                Title = m.Title,
                Type = m.Type,
                ScheduledDate = m.ScheduledDate,
                IsCompleted = m.IsCompleted
            }).ToList();
        }
    }
}
