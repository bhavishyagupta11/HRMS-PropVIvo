using MediatR;
using Microsoft.AspNetCore.Http;
using RecruitmentFeature.Application.DTO;
using RecruitmentFeature.Application.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RecruitmentFeature.Application.Queries
{
    public class GetMyApplicationsQuery : IRequest<IEnumerable<CandidateApplicationRecordDto>>
    {
    }

    public class GetMyApplicationsQueryHandler : IRequestHandler<GetMyApplicationsQuery, IEnumerable<CandidateApplicationRecordDto>>
    {
        private readonly IRecruitmentRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMyApplicationsQueryHandler(IRecruitmentRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<CandidateApplicationRecordDto>> Handle(GetMyApplicationsQuery request, CancellationToken cancellationToken)
        {
            var email = _httpContextAccessor.HttpContext?.User?.FindFirst("email")?.Value ?? "employee.premier@enterprise.hrms";
            var records = await _repository.GetApplicationsAsync(email);
            return records.Select(x => new CandidateApplicationRecordDto
            {
                Id = x.Id,
                JobId = x.JobId,
                ApplicantName = x.ApplicantName,
                ApplicantEmail = x.ApplicantEmail,
                ResumeBlobUrl = x.ResumeBlobUrl,
                Stage = x.Stage,
                Status = x.Status,
                AppliedDate = x.AppliedDate
            });
        }
    }
}
