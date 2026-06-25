using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using RecruitmentFeature.Application.Commands;
using RecruitmentFeature.Application.DTO;
using System.Threading.Tasks;

namespace RecruitmentFeature.GraphQL
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class RecruitmentMutation
    {
        [Authorize(Roles = new[] { "Admin", "HR" })]
        public async Task<CandidateApplicationRecordDto> ApplyForJob(ApplyForJobCommand command, [Service] IMediator mediator)
        {
            return await mediator.Send(command);
        }

        [Authorize(Roles = new[] { "Admin", "HR" })]
        public async Task<CandidateApplicationRecordDto> UpdateApplicationStage(UpdateApplicationStageCommand command, [Service] IMediator mediator)
        {
            return await mediator.Send(command);
        }
    }
}
