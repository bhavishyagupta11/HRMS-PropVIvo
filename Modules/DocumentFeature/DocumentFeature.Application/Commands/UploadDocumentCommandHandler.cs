using DocumentFeature.Application.DTO;
using DocumentFeature.Application.Repository;
using DocumentFeature.Domain;
using HRMS.Shared.Application.Services;
using HRMS.Shared.Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentFeature.Application.Commands
{
    public class UploadDocumentCommand : IRequest<DocumentRecordDto>
    {
        public UploadDocumentDto Payload { get; set; } = new();
    }

    public class UploadDocumentCommandHandler : IRequestHandler<UploadDocumentCommand, DocumentRecordDto>
    {
        private readonly IDocumentRepository _repository;
        private readonly IAzureStorage _azureStorage;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UploadDocumentCommandHandler(IDocumentRepository repository, IAzureStorage azureStorage, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _azureStorage = azureStorage;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<DocumentRecordDto> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                ?? user?.Claims?.FirstOrDefault(c => c.Type == "sub")?.Value 
                ?? string.Empty;
            if (string.IsNullOrEmpty(userId))
            {
                userId = "EMP12345";
            }

            var fileBytes = Convert.FromBase64String(request.Payload.Base64Content);
            var folderName = userId;
            var fileName = $"{Guid.NewGuid()}_{request.Payload.FileName}";

            var uploadResult = await _azureStorage.UploadAsync(
                BlobContainerNames.documents,
                folderName,
                fileName,
                fileBytes,
                request.Payload.FileType);

            var record = new DocumentRecord
            {
                UserId = userId,
                Category = request.Payload.Category,
                FileName = request.Payload.FileName,
                FileType = request.Payload.FileType,
                FileSize = request.Payload.FileSize,
                BlobUrl = uploadResult.Blob.Uri ?? $"https://storage.hrms.local/documents/{folderName}/{fileName}",
                UploadDate = DateTime.UtcNow,
                ExpiryDate = request.Payload.ExpiryDate,
                VerificationStatus = "Uploaded"
            };

            var saved = await _repository.AddAsync(record, cancellationToken);

            return new DocumentRecordDto
            {
                Id = saved.Id,
                UserId = saved.UserId,
                Category = saved.Category,
                FileName = saved.FileName,
                FileType = saved.FileType,
                FileSize = saved.FileSize,
                BlobUrl = saved.BlobUrl,
                UploadDate = saved.UploadDate,
                ExpiryDate = saved.ExpiryDate,
                VerificationStatus = saved.VerificationStatus,
                RejectionReason = saved.RejectionReason,
                VerificationDate = saved.VerificationDate
            };
        }
    }
}
