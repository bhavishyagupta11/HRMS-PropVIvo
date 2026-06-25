using System;

namespace ContributionsFeature.Application.DTO
{
    public class ValueContributionDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ContributionType { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int Points { get; set; }
        public int SuggestedPoints { get; set; }
        public string ImpactLevel { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public string ApproverName { get; set; } = string.Empty;
        public string ApprovalComments { get; set; } = string.Empty;
        public DateTime SubmittedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
    }

    public class ContributionItemDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int SuggestedPoints { get; set; }
        public string Status { get; set; } = string.Empty;
        public string ClaimedByEmployee { get; set; } = string.Empty;
    }

    public class ContributionLeaderboardDto
    {
        public string Id { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public int TotalPoints { get; set; }
        public string Badges { get; set; } = string.Empty;
        public double AverageRating { get; set; }
    }
}
