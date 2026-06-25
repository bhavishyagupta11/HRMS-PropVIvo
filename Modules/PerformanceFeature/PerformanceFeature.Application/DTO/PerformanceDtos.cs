using System;

namespace PerformanceFeature.Application.DTO
{
    public class GoalRecordDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime TargetDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal MetricTarget { get; set; }
        public decimal MetricActual { get; set; }
    }

    public class PerformanceReviewRecordDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string ReviewCycle { get; set; } = string.Empty;
        public decimal SelfRating { get; set; }
        public decimal ManagerRating { get; set; }
        public string Comments { get; set; } = string.Empty;
        public string ReviewerUserId { get; set; } = string.Empty;
    }
}
