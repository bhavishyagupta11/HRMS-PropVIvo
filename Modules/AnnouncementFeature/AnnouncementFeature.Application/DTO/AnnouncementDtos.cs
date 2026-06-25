using System;

namespace AnnouncementFeature.Application.DTO
{
    public class AnnouncementRecordDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string TargetAudience { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }
    }
}
