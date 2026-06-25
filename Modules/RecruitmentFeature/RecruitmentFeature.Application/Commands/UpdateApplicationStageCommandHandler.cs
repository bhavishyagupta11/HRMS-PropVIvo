using MediatR;
using RecruitmentFeature.Application.DTO;
using RecruitmentFeature.Application.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RecruitmentFeature.Application.Commands
{
    public class UpdateApplicationStageCommand : IRequest<CandidateApplicationRecordDto>
    {
        public string ApplicationId { get; set; } = string.Empty;
        public string Stage { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class UpdateApplicationStageCommandHandler : IRequestHandler<UpdateApplicationStageCommand, CandidateApplicationRecordDto>
    {
        private readonly IRecruitmentRepository _repository;

        public UpdateApplicationStageCommandHandler(IRecruitmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<CandidateApplicationRecordDto> Handle(UpdateApplicationStageCommand request, CancellationToken cancellationToken)
        {
            var record = await _repository.GetApplicationByIdAsync(request.ApplicationId);
            if (record == null)
            {
                throw new Exception("Application not found");
            }

            record.Stage = request.Stage;
            record.Status = request.Status;

            await _repository.UpdateApplicationAsync(record);

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
