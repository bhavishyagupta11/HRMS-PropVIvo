using HRMS.Core.Postgres.Common;
using System;

namespace CopilotFeature.Domain
{
    public class CopilotInteractionRecord : BaseEntity
    {
        public string PromptText { get; set; } = string.Empty;
        public string ResponseText { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public double ConfidenceScore { get; set; }
        public DateTime InteractionDate { get; set; }
    }
}
