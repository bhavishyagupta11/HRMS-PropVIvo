using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using PayrollFeature.Application.Commands;
using PayrollFeature.Application.DTO;
using System.Threading;
using System.Threading.Tasks;

namespace PayrollFeature.GraphQL.Mutations
{
    [ExtendObjectType("Mutation")]
    public class PayrollMutations
    {
        [Authorize(Roles = new[] { "Admin", "HR" })]
        public async Task<PayrollRecordDto> GeneratePayslip(
            GeneratePayslipDto input,
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            return await mediator.Send(new GeneratePayslipCommand { Payload = input }, cancellationToken);
        }
    }
}
