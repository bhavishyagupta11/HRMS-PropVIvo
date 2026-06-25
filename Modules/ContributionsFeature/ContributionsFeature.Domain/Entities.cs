using HRMS.Core.Postgres.Common;
using System;

namespace ContributionsFeature.Domain
{
    public class ValueContribution : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ContributionType { get; set; } = string.Empty; // self-initiated, committed, assigned
        public string Category { get; set; } = string.Empty; // innovation, process-improvement, cost-saving, etc.
        public string Status { get; set; } = string.Empty; // draft, proposal-pending, approved-to-start, in-progress, under-review, completed, rejected
        public int Points { get; set; }
        public int SuggestedPoints { get; set; }
        public string ImpactLevel { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public string ApproverName { get; set; } = string.Empty;
        public string ApprovalComments { get; set; } = string.Empty;
        public DateTime SubmittedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
    }

    public class ContributionItem : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int SuggestedPoints { get; set; }
        public string Status { get; set; } = string.Empty; // Available, Claimed
        public string ClaimedByEmployee { get; set; } = string.Empty;
    }

    public class ContributionLeaderboard : BaseEntity
    {
        public string EmployeeName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public int TotalPoints { get; set; }
        public string Badges { get; set; } = string.Empty; // e.g. "Gold Champion, Innovation Star"
        public double AverageRating { get; set; }
    }
}
