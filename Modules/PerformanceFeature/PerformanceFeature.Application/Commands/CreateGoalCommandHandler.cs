using MediatR;
using Microsoft.AspNetCore.Http;
using PerformanceFeature.Application.DTO;
using PerformanceFeature.Application.Repository;
using PerformanceFeature.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PerformanceFeature.Application.Commands
{
    public class CreateGoalCommand : IRequest<GoalRecordDto>
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime TargetDate { get; set; }
        public decimal MetricTarget { get; set; }
    }

    public class CreateGoalCommandHandler : IRequestHandler<CreateGoalCommand, GoalRecordDto>
    {
        private readonly IPerformanceRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateGoalCommandHandler(IPerformanceRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GoalRecordDto> Handle(CreateGoalCommand request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
                ?? user?.FindFirst("sub")?.Value 
                ?? "USR-9988231-HRMS";
            var record = new GoalRecord
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                Title = request.Title,
                Description = request.Description,
                TargetDate = request.TargetDate,
                Status = "InProgress",
                MetricTarget = request.MetricTarget,
                MetricActual = 0
            };

            await _repository.CreateGoalAsync(record);

            return new GoalRecordDto
            {
                Id = record.Id,
                UserId = record.UserId,
                Title = record.Title,
                Description = record.Description,
                TargetDate = record.TargetDate,
                Status = record.Status,
                MetricTarget = record.MetricTarget,
                MetricActual = record.MetricActual
            };
        }
    }
}
