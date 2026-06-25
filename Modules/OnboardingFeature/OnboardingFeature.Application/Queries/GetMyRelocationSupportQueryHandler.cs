using MediatR;
using OnboardingFeature.Application.DTO;
using OnboardingFeature.Application.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace OnboardingFeature.Application.Queries
{
    // ON-04: Relocation support details
    public class GetMyRelocationSupportQuery : IRequest<RelocationSupportDto?>
    {
        public string OnboardingEmployeeId { get; set; } = string.Empty;
    }

    public class GetMyRelocationSupportQueryHandler : IRequestHandler<GetMyRelocationSupportQuery, RelocationSupportDto?>
    {
        private readonly IOnboardingRepository _repository;

        public GetMyRelocationSupportQueryHandler(IOnboardingRepository repository)
        {
            _repository = repository;
        }

        public async Task<RelocationSupportDto?> Handle(GetMyRelocationSupportQuery request, CancellationToken cancellationToken)
        {
            var relocation = await _repository.GetRelocationSupportByEmployeeIdAsync(request.OnboardingEmployeeId);
            if (relocation == null) return null;

            return new RelocationSupportDto
            {
                Id = relocation.Id,
                RelocationStatus = relocation.RelocationStatus,
                VisaStatus = relocation.VisaStatus,
                Accommodation = relocation.Accommodation,
                TravelDetails = relocation.TravelDetails,
                Allowance = relocation.Allowance,
                LocalBuddyContact = relocation.LocalBuddyContact,
                SupportTickets = relocation.SupportTickets
            };
        }
    }
}
