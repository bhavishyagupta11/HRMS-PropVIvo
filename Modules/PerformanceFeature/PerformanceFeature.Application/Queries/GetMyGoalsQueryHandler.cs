using MediatR;
using Microsoft.AspNetCore.Http;
using PerformanceFeature.Application.DTO;
using PerformanceFeature.Application.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PerformanceFeature.Application.Queries
{
    public class GetMyGoalsQuery : IRequest<IEnumerable<GoalRecordDto>>
    {
    }

    public class GetMyGoalsQueryHandler : IRequestHandler<GetMyGoalsQuery, IEnumerable<GoalRecordDto>>
    {
        private readonly IPerformanceRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMyGoalsQueryHandler(IPerformanceRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<GoalRecordDto>> Handle(GetMyGoalsQuery request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
                ?? user?.FindFirst("sub")?.Value 
                ?? "USR-9988231-HRMS";
            var records = await _repository.GetGoalsAsync(userId);
            return records.Select(x => new GoalRecordDto
            {
                Id = x.Id,
                UserId = x.UserId,
                Title = x.Title,
                Description = x.Description,
                TargetDate = x.TargetDate,
                Status = x.Status,
                MetricTarget = x.MetricTarget,
                MetricActual = x.MetricActual
            });
        }
    }
}
