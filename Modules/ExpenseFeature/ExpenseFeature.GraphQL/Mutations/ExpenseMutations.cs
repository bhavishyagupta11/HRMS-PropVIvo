using ExpenseFeature.Application.Commands;
using ExpenseFeature.Application.DTO;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ExpenseFeature.GraphQL.Mutations
{
    [ExtendObjectType("Mutation")]
    public class ExpenseMutations
    {
        [Authorize(Roles = new[] { "Employee", "Admin", "HR", "Manager", "ReportingManager" })]
        public async Task<ReimbursementRecordDto> SubmitExpense(
            SubmitExpenseDto input,
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            return await mediator.Send(new SubmitExpenseCommand { Payload = input }, cancellationToken);
        }

        [Authorize(Roles = new[] { "Admin", "HR", "Manager", "ReportingManager" })]
        public async Task<ReimbursementRecordDto> ProcessExpense(
            ProcessExpenseDto input,
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            return await mediator.Send(new ProcessExpenseCommand { Payload = input }, cancellationToken);
        }
    }
}
