using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrainingFeature.Application.DTO;
using TrainingFeature.Application.Repository;

namespace TrainingFeature.Application.Queries
{
    public class GetMyEnrollmentsQuery : IRequest<IEnumerable<CourseEnrollmentRecordDto>>
    {
    }

    public class GetMyEnrollmentsQueryHandler : IRequestHandler<GetMyEnrollmentsQuery, IEnumerable<CourseEnrollmentRecordDto>>
    {
        private readonly ITrainingRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMyEnrollmentsQueryHandler(ITrainingRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<CourseEnrollmentRecordDto>> Handle(GetMyEnrollmentsQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value ?? "USR-9988231-HRMS";
            var records = await _repository.GetMyEnrollmentsAsync(userId);
            return records.Select(x => new CourseEnrollmentRecordDto
            {
                Id = x.Id,
                CourseId = x.CourseId,
                UserId = x.UserId,
                Status = x.Status,
                EnrollmentDate = x.EnrollmentDate,
                CompletionDate = x.CompletionDate
            });
        }
    }
}
