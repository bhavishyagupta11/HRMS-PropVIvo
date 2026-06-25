using System;

namespace RecognitionFeature.Application.DTO
{
    public class RecognitionRecordDto
    {
        public string Id { get; set; } = string.Empty;
        public string GiverName { get; set; } = string.Empty;
        public string ReceiverName { get; set; } = string.Empty;
        public string ReceiverEmail { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int Points { get; set; }
        public DateTime AwardedDate { get; set; }
    }
}
