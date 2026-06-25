using System;

namespace DocumentFeature.Application.DTO
{
    public class DocumentRecordDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string BlobUrl { get; set; } = string.Empty;
        public DateTime UploadDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string VerificationStatus { get; set; } = string.Empty;
        public string? RejectionReason { get; set; }
        public DateTime? VerificationDate { get; set; }
    }

    public class UploadDocumentDto
    {
        public string Category { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string Base64Content { get; set; } = string.Empty;
        public DateTime? ExpiryDate { get; set; }
    }

    public class VerifyDocumentDto
    {
        public string DocumentId { get; set; } = string.Empty;
        public bool IsApproved { get; set; }
        public string? RejectionReason { get; set; }
    }
}
