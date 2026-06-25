using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;
using TrainingFeature.Application.DTO;
using TrainingFeature.Application.Repository;

namespace TrainingFeature.Application.Commands
{
    public class CompleteCourseCommand : IRequest<CourseEnrollmentRecordDto>
    {
        public string EnrollmentId { get; set; } = string.Empty;
    }

    public class CompleteCourseCommandHandler : IRequestHandler<CompleteCourseCommand, CourseEnrollmentRecordDto>
    {
        private readonly ITrainingRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CompleteCourseCommandHandler(ITrainingRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CourseEnrollmentRecordDto> Handle(CompleteCourseCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value ?? "USR-9988231-HRMS";
            var record = await _repository.GetEnrollmentAsync(request.EnrollmentId, userId);
            if (record == null)
            {
                throw new Exception("Enrollment not found");
            }

            record.Status = "Completed";
            record.CompletionDate = DateTime.UtcNow;

            await _repository.UpdateEnrollmentAsync(record);

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
