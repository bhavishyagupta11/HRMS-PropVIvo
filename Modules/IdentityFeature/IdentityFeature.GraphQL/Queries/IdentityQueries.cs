using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using IdentityFeature.Application.DTO;
using IdentityFeature.Application.Queries;
using MediatR;

namespace IdentityFeature.GraphQL.Queries
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class IdentityQueries
    {
        [Authorize]
        public async Task<IEnumerable<TeamMemberDto>> GetTeamMembers([Service] IMediator mediator)
        {
            return await mediator.Send(new GetTeamMembersQuery());
        }
    }
}
