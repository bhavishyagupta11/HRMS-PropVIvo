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
    public class GetMyReviewsQuery : IRequest<IEnumerable<PerformanceReviewRecordDto>>
    {
    }

    public class GetMyReviewsQueryHandler : IRequestHandler<GetMyReviewsQuery, IEnumerable<PerformanceReviewRecordDto>>
    {
        private readonly IPerformanceRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMyReviewsQueryHandler(IPerformanceRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<PerformanceReviewRecordDto>> Handle(GetMyReviewsQuery request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
                ?? user?.FindFirst("sub")?.Value 
                ?? "USR-9988231-HRMS";
            var records = await _repository.GetReviewsAsync(userId);
            return records.Select(x => new PerformanceReviewRecordDto
            {
                Id = x.Id,
                UserId = x.UserId,
                ReviewCycle = x.ReviewCycle,
                SelfRating = x.SelfRating,
                ManagerRating = x.ManagerRating,
                Comments = x.Comments,
                ReviewerUserId = x.ReviewerUserId
            });
        }
    }
}
