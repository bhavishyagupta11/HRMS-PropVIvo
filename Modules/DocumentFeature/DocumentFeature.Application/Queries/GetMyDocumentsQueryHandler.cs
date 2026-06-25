using DocumentFeature.Application.DTO;
using DocumentFeature.Application.Repository;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentFeature.Application.Queries
{
    public class GetMyDocumentsQuery : IRequest<IEnumerable<DocumentRecordDto>> { }

    public class GetMyDocumentsQueryHandler : IRequestHandler<GetMyDocumentsQuery, IEnumerable<DocumentRecordDto>>
    {
        private readonly IDocumentRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMyDocumentsQueryHandler(IDocumentRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<DocumentRecordDto>> Handle(GetMyDocumentsQuery request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                ?? user?.Claims?.FirstOrDefault(c => c.Type == "sub")?.Value 
                ?? string.Empty;
            if (string.IsNullOrEmpty(userId))
            {
                userId = "EMP12345";
            }

            var records = await _repository.GetByUserIdAsync(userId, cancellationToken);

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
