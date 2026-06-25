using DocumentFeature.Application.Commands;
using DocumentFeature.Application.DTO;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentFeature.GraphQL.Mutations
{
    [ExtendObjectType("Mutation")]
    public class DocumentMutations
    {
        [Authorize(Roles = new[] { "Employee", "Admin", "HR", "Manager", "ReportingManager" })]
        public async Task<DocumentRecordDto> UploadDocument(
            UploadDocumentDto input,
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            return await mediator.Send(new UploadDocumentCommand { Payload = input }, cancellationToken);
        }

        [Authorize(Roles = new[] { "Admin", "HR" })]
        public async Task<DocumentRecordDto> VerifyDocument(
            VerifyDocumentDto input,
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            return await mediator.Send(new VerifyDocumentCommand { Payload = input }, cancellationToken);
        }
    }
}
