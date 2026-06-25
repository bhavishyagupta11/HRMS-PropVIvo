using MediatR;
using OnboardingFeature.Application.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace OnboardingFeature.Application.Commands
{
    // ON-06: "As a new joiner, I want to mark my onboarding complete so that I transition
    //         into the standard employee experience."
    // ON-06 AC: "Completing onboarding hides the onboarding flow."
    //           "The user lands on the standard home dashboard afterward."
    public class CompleteOnboardingCommand : IRequest<bool>
    {
        public string UserId { get; set; } = string.Empty;
    }

    public class CompleteOnboardingCommandHandler : IRequestHandler<CompleteOnboardingCommand, bool>
    {
        private readonly IOnboardingRepository _repository;

        public CompleteOnboardingCommandHandler(IOnboardingRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(CompleteOnboardingCommand request, CancellationToken cancellationToken)
        {
            var employee = await _repository.GetByUserIdAsync(request.UserId);
            if (employee == null)
                throw new KeyNotFoundException("Onboarding record not found for this user.");

            if (employee.IsCompleted)
                throw new InvalidOperationException("Onboarding is already completed.");

            employee.IsCompleted = true;
            employee.OverallProgressPercent = 100;
            await _repository.UpdateEmployeeAsync(employee);

            return true;
        }
    }
}
