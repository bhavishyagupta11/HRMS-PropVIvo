using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;
using TrainingFeature.Application.DTO;
using TrainingFeature.Application.Repository;
using TrainingFeature.Domain;

namespace TrainingFeature.Application.Commands
{
    public class EnrollInCourseCommand : IRequest<CourseEnrollmentRecordDto>
    {
        public string CourseId { get; set; } = string.Empty;
    }

    public class EnrollInCourseCommandHandler : IRequestHandler<EnrollInCourseCommand, CourseEnrollmentRecordDto>
    {
        private readonly ITrainingRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EnrollInCourseCommandHandler(ITrainingRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CourseEnrollmentRecordDto> Handle(EnrollInCourseCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value ?? "USR-9988231-HRMS";
            var record = new CourseEnrollmentRecord
            {
                Id = Guid.NewGuid().ToString(),
                CourseId = request.CourseId,
                UserId = userId,
                Status = "Enrolled",
                EnrollmentDate = DateTime.UtcNow,
                CompletionDate = null
            };

            await _repository.EnrollAsync(record);

            return new CourseEnrollmentRecordDto
            {
                Id = record.Id,
                CourseId = record.CourseId,
                UserId = record.UserId,
                Status = record.Status,
                EnrollmentDate = record.EnrollmentDate,
                CompletionDate = record.CompletionDate
            };
        }
    }
}
