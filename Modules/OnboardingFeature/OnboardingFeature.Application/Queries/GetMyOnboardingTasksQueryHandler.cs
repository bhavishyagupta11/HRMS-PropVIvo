using MediatR;
using OnboardingFeature.Application.DTO;
using OnboardingFeature.Application.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnboardingFeature.Application.Queries
{
    // ON-01/02: "Tasks are grouped by phase with clear status badges."
    // Optional phase filter — if null, returns all tasks for the employee.
    public class GetMyOnboardingTasksQuery : IRequest<List<OnboardingTaskDto>>
    {
        public string OnboardingEmployeeId { get; set; } = string.Empty;
        public string? Phase { get; set; }
    }

    public class GetMyOnboardingTasksQueryHandler : IRequestHandler<GetMyOnboardingTasksQuery, List<OnboardingTaskDto>>
    {
        private readonly IOnboardingRepository _repository;

        public GetMyOnboardingTasksQueryHandler(IOnboardingRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<OnboardingTaskDto>> Handle(GetMyOnboardingTasksQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _repository.GetTasksByEmployeeIdAsync(request.OnboardingEmployeeId);

            if (!string.IsNullOrEmpty(request.Phase))
                tasks = tasks.Where(t => t.Phase == request.Phase).ToList();

            return tasks.Select(t => new OnboardingTaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Phase = t.Phase,
                Priority = t.Priority,
                DueDate = t.DueDate,
                Assignee = t.Assignee,
                Status = t.Status,
                CompletedDate = t.CompletedDate
            }).ToList();
        }
    }
}
