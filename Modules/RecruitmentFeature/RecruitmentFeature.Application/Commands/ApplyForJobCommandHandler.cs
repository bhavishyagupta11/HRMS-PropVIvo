using MediatR;
using Microsoft.AspNetCore.Http;
using RecruitmentFeature.Application.DTO;
using RecruitmentFeature.Application.Repository;
using RecruitmentFeature.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RecruitmentFeature.Application.Commands
{
    public class ApplyForJobCommand : IRequest<CandidateApplicationRecordDto>
    {
        public string JobId { get; set; } = string.Empty;
        public string ApplicantName { get; set; } = string.Empty;
        public string ResumeBlobUrl { get; set; } = string.Empty;
    }

    public class ApplyForJobCommandHandler : IRequestHandler<ApplyForJobCommand, CandidateApplicationRecordDto>
    {
        private readonly IRecruitmentRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplyForJobCommandHandler(IRecruitmentRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CandidateApplicationRecordDto> Handle(ApplyForJobCommand request, CancellationToken cancellationToken)
        {
            var email = _httpContextAccessor.HttpContext?.User?.FindFirst("email")?.Value ?? "employee.premier@enterprise.hrms";
            var record = new CandidateApplicationRecord
            {
                Id = Guid.NewGuid().ToString(),
                JobId = request.JobId,
                ApplicantName = request.ApplicantName,
                ApplicantEmail = email,
                ResumeBlobUrl = request.ResumeBlobUrl,
                Stage = "Screening",
                Status = "Applied",
                AppliedDate = DateTime.UtcNow
            };

            await _repository.ApplyAsync(record);

            return new CandidateApplicationRecordDto
            {
                Id = record.Id,
                JobId = record.JobId,
                ApplicantName = record.ApplicantName,
                ApplicantEmail = record.ApplicantEmail,
                ResumeBlobUrl = record.ResumeBlobUrl,
                Stage = record.Stage,
                Status = record.Status,
                AppliedDate = record.AppliedDate
            };
        }
    }
}
