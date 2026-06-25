using DocumentFeature.Application.DTO;
using DocumentFeature.Application.Queries;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentFeature.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class DocumentQueries
    {
        [Authorize(Roles = new[] { "Employee", "Admin", "HR", "Manager", "ReportingManager" })]
        public async Task<IEnumerable<DocumentRecordDto>> GetMyDocuments(
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetMyDocumentsQuery(), cancellationToken);
        }

        [Authorize(Roles = new[] { "Admin", "HR" })]
        public async Task<IEnumerable<DocumentRecordDto>> GetPendingDocuments(
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetPendingDocumentsQuery(), cancellationToken);
        }
    }
}
