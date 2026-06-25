using HRMS.Shared.Domain.Entity;
using HRMS.Core.Postgres.Common;
using System;

namespace DocumentFeature.Domain
{
    public class DocumentRecord : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // identity, employment, work-auth, tax, education, other
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string BlobUrl { get; set; } = string.Empty;
        public DateTime UploadDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string VerificationStatus { get; set; } = "Uploaded"; // missing, uploaded, verified, rejected
        public string? RejectionReason { get; set; }
        public DateTime? VerificationDate { get; set; }
    }
}
