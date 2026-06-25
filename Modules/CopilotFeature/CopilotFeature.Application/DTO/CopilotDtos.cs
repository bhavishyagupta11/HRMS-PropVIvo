using System;

namespace CopilotFeature.Application.DTO
{
    public class CopilotInteractionRecordDto
    {
        public string Id { get; set; } = string.Empty;
        public string PromptText { get; set; } = string.Empty;
        public string ResponseText { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public double ConfidenceScore { get; set; }
        public DateTime InteractionDate { get; set; }
    }
}
