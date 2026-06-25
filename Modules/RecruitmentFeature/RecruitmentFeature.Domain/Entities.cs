using HRMS.Core.Postgres.Common;
using System;

namespace RecruitmentFeature.Domain
{
    public class JobRequisitionRecord : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Requirements { get; set; } = string.Empty;
    }

    public class CandidateApplicationRecord : BaseEntity
    {
        public string JobId { get; set; } = string.Empty;
        public string ApplicantName { get; set; } = string.Empty;
        public string ApplicantEmail { get; set; } = string.Empty;
        public string ResumeBlobUrl { get; set; } = string.Empty;
        public string Stage { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime AppliedDate { get; set; }
    }
}
