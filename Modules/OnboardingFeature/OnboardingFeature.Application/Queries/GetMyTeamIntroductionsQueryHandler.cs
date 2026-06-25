using MediatR;
using OnboardingFeature.Application.DTO;
using OnboardingFeature.Application.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnboardingFeature.Application.Queries
{
    // ON-05: "Team member cards show bio, expertise, and fun facts."
    public class GetMyTeamIntroductionsQuery : IRequest<List<TeamIntroductionDto>>
    {
        public string OnboardingEmployeeId { get; set; } = string.Empty;
    }

    public class GetMyTeamIntroductionsQueryHandler : IRequestHandler<GetMyTeamIntroductionsQuery, List<TeamIntroductionDto>>
    {
        private readonly IOnboardingRepository _repository;

        public GetMyTeamIntroductionsQueryHandler(IOnboardingRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<TeamIntroductionDto>> Handle(GetMyTeamIntroductionsQuery request, CancellationToken cancellationToken)
        {
            var introductions = await _repository.GetTeamIntroductionsByEmployeeIdAsync(request.OnboardingEmployeeId);

            return introductions.Select(i => new TeamIntroductionDto
            {
                Id = i.Id,
                TeamMemberName = i.TeamMemberName,
                Bio = i.Bio,
                Expertise = i.Expertise,
                FunFact = i.FunFact,
                IntroductionStatus = i.IntroductionStatus
            }).ToList();
        }
    }
}
