using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using PayrollFeature.Application.DTO;
using PayrollFeature.Application.Queries;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PayrollFeature.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class PayrollQueries
    {
        [Authorize(Roles = new[] { "Employee", "Admin", "HR", "Manager", "ReportingManager" })]
        public async Task<IEnumerable<PayrollRecordDto>> GetMyPayslips(
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetMyPayslipsQuery(), cancellationToken);
        }
    }
}
