using AnnouncementFeature.Application.DTO;
using AnnouncementFeature.Application.Repository;
using AnnouncementFeature.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnnouncementFeature.Application.Commands
{
    public class PublishAnnouncementCommand : IRequest<AnnouncementRecordDto>
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string TargetAudience { get; set; } = string.Empty;
    }

    public class PublishAnnouncementCommandHandler : IRequestHandler<PublishAnnouncementCommand, AnnouncementRecordDto>
    {
        private readonly IAnnouncementRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PublishAnnouncementCommandHandler(IAnnouncementRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AnnouncementRecordDto> Handle(PublishAnnouncementCommand request, CancellationToken cancellationToken)
        {
            var authorName = _httpContextAccessor.HttpContext?.User?.FindFirst("name")?.Value ?? "Corporate Communications";
            var record = new AnnouncementRecord
            {
                Id = Guid.NewGuid().ToString(),
                Title = request.Title,
                Content = request.Content,
                AuthorName = authorName,
                Priority = request.Priority,
                TargetAudience = request.TargetAudience,
                PublishedDate = DateTime.UtcNow
            };

            await _repository.PublishAnnouncementAsync(record);

            return new AnnouncementRecordDto
            {
                Id = record.Id,
                Title = record.Title,
                Content = record.Content,
                AuthorName = record.AuthorName,
                Priority = record.Priority,
                TargetAudience = record.TargetAudience,
                PublishedDate = record.PublishedDate
            };
        }
    }
}
