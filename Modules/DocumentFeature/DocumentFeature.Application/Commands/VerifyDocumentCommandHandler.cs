using DocumentFeature.Application.DTO;
using DocumentFeature.Application.Repository;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentFeature.Application.Commands
{
    public class VerifyDocumentCommand : IRequest<DocumentRecordDto>
    {
        public VerifyDocumentDto Payload { get; set; } = new();
    }

    public class VerifyDocumentCommandHandler : IRequestHandler<VerifyDocumentCommand, DocumentRecordDto>
    {
        private readonly IDocumentRepository _repository;

        public VerifyDocumentCommandHandler(IDocumentRepository repository)
        {
            _repository = repository;
        }

        public async Task<DocumentRecordDto> Handle(VerifyDocumentCommand request, CancellationToken cancellationToken)
        {
            var record = await _repository.GetByIdAsync(request.Payload.DocumentId, cancellationToken);
            if (record == null)
            {
                throw new Exception("Document not found.");
            }

            if (request.Payload.IsApproved)
            {
                record.VerificationStatus = "Verified";
                record.VerificationDate = DateTime.UtcNow;
            }
            else
            {
                record.VerificationStatus = "Rejected";
                record.RejectionReason = request.Payload.RejectionReason;
            }

            var updated = await _repository.UpdateAsync(record, cancellationToken);

            return new DocumentRecordDto
            {
                Id = updated.Id,
                UserId = updated.UserId,
                Category = updated.Category,
                FileName = updated.FileName,
                FileType = updated.FileType,
                FileSize = updated.FileSize,
                BlobUrl = updated.BlobUrl,
                UploadDate = updated.UploadDate,
                ExpiryDate = updated.ExpiryDate,
                VerificationStatus = updated.VerificationStatus,
                RejectionReason = updated.RejectionReason,
                VerificationDate = updated.VerificationDate
            };
        }
    }
}
