using MediatR;
using OnboardingFeature.Application.DTO;
using OnboardingFeature.Application.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnboardingFeature.Application.Queries
{
    // ON-03: "Welcome messages display sender name, role, and message."
    // ON-03: "Video welcome messages can be played when available."
    public class GetMyWelcomeMessagesQuery : IRequest<List<WelcomeMessageDto>>
    {
        public string OnboardingEmployeeId { get; set; } = string.Empty;
    }

    public class GetMyWelcomeMessagesQueryHandler : IRequestHandler<GetMyWelcomeMessagesQuery, List<WelcomeMessageDto>>
    {
        private readonly IOnboardingRepository _repository;

        public GetMyWelcomeMessagesQueryHandler(IOnboardingRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<WelcomeMessageDto>> Handle(GetMyWelcomeMessagesQuery request, CancellationToken cancellationToken)
        {
            var messages = await _repository.GetWelcomeMessagesByEmployeeIdAsync(request.OnboardingEmployeeId);

            return messages.Select(m => new WelcomeMessageDto
            {
                Id = m.Id,
                SenderName = m.SenderName,
                SenderRole = m.SenderRole,
                Message = m.Message,
                VideoUrl = m.VideoUrl
            }).ToList();
        }
    }
}
