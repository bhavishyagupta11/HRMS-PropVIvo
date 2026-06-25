using MediatR;
using RecruitmentFeature.Application.DTO;
using RecruitmentFeature.Application.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RecruitmentFeature.Application.Queries
{
    public class GetOpenJobsQuery : IRequest<IEnumerable<JobRequisitionRecordDto>>
    {
    }

    public class GetOpenJobsQueryHandler : IRequestHandler<GetOpenJobsQuery, IEnumerable<JobRequisitionRecordDto>>
    {
        private readonly IRecruitmentRepository _repository;

        public GetOpenJobsQueryHandler(IRecruitmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<JobRequisitionRecordDto>> Handle(GetOpenJobsQuery request, CancellationToken cancellationToken)
        {
            var records = await _repository.GetOpenJobsAsync();
            return records.Select(x => new JobRequisitionRecordDto
            {
                Id = x.Id,
                Title = x.Title,
                Department = x.Department,
                Location = x.Location,
                Description = x.Description,
                Requirements = x.Requirements
            });
        }
    }
}
