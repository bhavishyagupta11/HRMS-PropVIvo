using MediatR;
using OnboardingFeature.Application.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnboardingFeature.Application.Commands
{
    // ON-02: "I can mark employee-assigned tasks as complete."
    // ON-02: "Completed tasks display a completion date."
    public class CompleteOnboardingTaskCommand : IRequest<bool>
    {
        public string TaskId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }

    public class CompleteOnboardingTaskCommandHandler : IRequestHandler<CompleteOnboardingTaskCommand, bool>
    {
        private readonly IOnboardingRepository _repository;

        public CompleteOnboardingTaskCommandHandler(IOnboardingRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(CompleteOnboardingTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _repository.GetTaskByIdAsync(request.TaskId);
            if (task == null)
                throw new KeyNotFoundException($"Onboarding task '{request.TaskId}' not found.");

            // Validate the task belongs to the calling user's onboarding record
            var employee = await _repository.GetByUserIdAsync(request.UserId);
            if (employee == null || task.OnboardingEmployeeId != employee.Id)
                throw new UnauthorizedAccessException("Task does not belong to this user.");

            if (task.Status == "Completed")
                throw new InvalidOperationException("Task is already completed.");

            task.Status = "Completed";
            task.CompletedDate = DateTime.UtcNow;

            await _repository.UpdateTaskAsync(task);

            // Recalculate progress on the employee record
            var allTasks = await _repository.GetTasksByEmployeeIdAsync(employee.Id);
            var completedCount = allTasks.Count(t => t.Status == "Completed");
            var totalCount = allTasks.Count;
            employee.OverallProgressPercent = totalCount > 0 ? (completedCount * 100 / totalCount) : 0;
            await _repository.UpdateEmployeeAsync(employee);

            return true;
        }
    }
}
