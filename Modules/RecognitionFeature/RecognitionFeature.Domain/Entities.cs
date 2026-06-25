using HRMS.Core.Postgres.Common;
using System;

namespace RecognitionFeature.Domain
{
    public class RecognitionRecord : BaseEntity
    {
        public string GiverName { get; set; } = string.Empty;
        public string ReceiverName { get; set; } = string.Empty;
        public string ReceiverEmail { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int Points { get; set; }
        public DateTime AwardedDate { get; set; }
    }
}
