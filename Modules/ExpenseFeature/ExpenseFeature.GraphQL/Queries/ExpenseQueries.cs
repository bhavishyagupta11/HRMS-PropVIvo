using ExpenseFeature.Application.DTO;
using ExpenseFeature.Application.Queries;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExpenseFeature.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class ExpenseQueries
    {
        [Authorize(Roles = new[] { "Employee", "Admin", "HR", "Manager", "ReportingManager" })]
        public async Task<IEnumerable<ReimbursementRecordDto>> GetMyExpenses(
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetMyExpensesQuery(), cancellationToken);
        }

        [Authorize(Roles = new[] { "Admin", "HR", "Manager", "ReportingManager" })]
        public async Task<IEnumerable<ReimbursementRecordDto>> GetPendingExpenses(
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetPendingExpensesQuery(), cancellationToken);
        }
    }
}
