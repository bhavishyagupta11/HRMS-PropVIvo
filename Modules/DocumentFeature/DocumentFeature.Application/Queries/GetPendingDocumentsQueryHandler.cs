using DocumentFeature.Application.DTO;
using DocumentFeature.Application.Repository;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentFeature.Application.Queries
{
    public class GetPendingDocumentsQuery : IRequest<IEnumerable<DocumentRecordDto>> { }

    public class GetPendingDocumentsQueryHandler : IRequestHandler<GetPendingDocumentsQuery, IEnumerable<DocumentRecordDto>>
    {
        private readonly IDocumentRepository _repository;

        public GetPendingDocumentsQueryHandler(IDocumentRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DocumentRecordDto>> Handle(GetPendingDocumentsQuery request, CancellationToken cancellationToken)
        {
            var records = await _repository.GetPendingDocumentsAsync(cancellationToken);

            return records.Select(r => new DocumentRecordDto
            {
                Id = r.Id,
                UserId = r.UserId,
                Category = r.Category,
                FileName = r.FileName,
                FileType = r.FileType,
                FileSize = r.FileSize,
                BlobUrl = r.BlobUrl,
                UploadDate = r.UploadDate,
                ExpiryDate = r.ExpiryDate,
                VerificationStatus = r.VerificationStatus,
                RejectionReason = r.RejectionReason,
                VerificationDate = r.VerificationDate
            });
        }
    }
}
