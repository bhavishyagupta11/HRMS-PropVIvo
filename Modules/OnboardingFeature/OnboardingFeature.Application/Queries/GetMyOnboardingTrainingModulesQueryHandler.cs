using MediatR;
using OnboardingFeature.Application.DTO;
using OnboardingFeature.Application.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnboardingFeature.Application.Queries
{
    // PSD Key Capabilities: "Mandatory and optional training modules with progress and certificates."
    // Minimum reference — full training module belongs to PSD Section 4.9.
    public class GetMyOnboardingTrainingModulesQuery : IRequest<List<OnboardingTrainingModuleRefDto>>
    {
        public string OnboardingEmployeeId { get; set; } = string.Empty;
    }

    public class GetMyOnboardingTrainingModulesQueryHandler : IRequestHandler<GetMyOnboardingTrainingModulesQuery, List<OnboardingTrainingModuleRefDto>>
    {
        private readonly IOnboardingRepository _repository;

        public GetMyOnboardingTrainingModulesQueryHandler(IOnboardingRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<OnboardingTrainingModuleRefDto>> Handle(GetMyOnboardingTrainingModulesQuery request, CancellationToken cancellationToken)
        {
            var modules = await _repository.GetTrainingModuleRefsByEmployeeIdAsync(request.OnboardingEmployeeId);

            return modules.Select(m => new OnboardingTrainingModuleRefDto
            {
                Id = m.Id,
                Title = m.Title,
                IsMandatory = m.IsMandatory,
                ProgressPercent = m.ProgressPercent,
                HasCertificate = m.HasCertificate
            }).ToList();
        }
    }
}
