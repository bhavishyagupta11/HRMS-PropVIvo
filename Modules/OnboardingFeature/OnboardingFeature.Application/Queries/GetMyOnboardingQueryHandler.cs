using MediatR;
using OnboardingFeature.Application.DTO;
using OnboardingFeature.Application.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace OnboardingFeature.Application.Queries
{
    // ON-01: Personalized onboarding dashboard — returns the employee's onboarding record
    public class GetMyOnboardingQuery : IRequest<OnboardingEmployeeDto?>
    {
        public string UserId { get; set; } = string.Empty;
    }

    public class GetMyOnboardingQueryHandler : IRequestHandler<GetMyOnboardingQuery, OnboardingEmployeeDto?>
    {
        private readonly IOnboardingRepository _repository;

        public GetMyOnboardingQueryHandler(IOnboardingRepository repository)
        {
            _repository = repository;
        }

        public async Task<OnboardingEmployeeDto?> Handle(GetMyOnboardingQuery request, CancellationToken cancellationToken)
        {
            var employee = await _repository.GetByUserIdAsync(request.UserId);
            if (employee == null) return null;

            return new OnboardingEmployeeDto
            {
                Id = employee.Id,
                UserId = employee.UserId,
                Designation = employee.Designation,
                Department = employee.Department,
                ManagerName = employee.ManagerName,
                BuddyName = employee.BuddyName,
                JoiningDate = employee.JoiningDate,
                OverallProgressPercent = employee.OverallProgressPercent,
                IsCompleted = employee.IsCompleted
            };
        }
    }
}
