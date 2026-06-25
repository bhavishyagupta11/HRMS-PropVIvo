using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using RecruitmentFeature.Application.DTO;
using RecruitmentFeature.Application.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecruitmentFeature.GraphQL
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class RecruitmentQuery
    {
        [Authorize(Roles = new[] { "Admin", "HR" })]
        public async Task<IEnumerable<JobRequisitionRecordDto>> GetOpenJobs([Service] IMediator mediator)
        {
            return await mediator.Send(new GetOpenJobsQuery());
        }

        [Authorize(Roles = new[] { "Admin", "HR" })]
        public async Task<IEnumerable<CandidateApplicationRecordDto>> GetMyApplications([Service] IMediator mediator)
        {
            return await mediator.Send(new GetMyApplicationsQuery());
        }
    }
}
