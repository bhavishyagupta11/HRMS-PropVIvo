using MediatR;
using Microsoft.AspNetCore.Http;
using RecognitionFeature.Application.DTO;
using RecognitionFeature.Application.Repository;
using RecognitionFeature.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RecognitionFeature.Application.Commands
{
    public class SendRecognitionCommand : IRequest<RecognitionRecordDto>
    {
        public string ReceiverName { get; set; } = string.Empty;
        public string ReceiverEmail { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int Points { get; set; }
    }

    public class SendRecognitionCommandHandler : IRequestHandler<SendRecognitionCommand, RecognitionRecordDto>
    {
        private readonly IRecognitionRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SendRecognitionCommandHandler(IRecognitionRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<RecognitionRecordDto> Handle(SendRecognitionCommand request, CancellationToken cancellationToken)
        {
            var giverName = _httpContextAccessor.HttpContext?.User?.FindFirst("name")?.Value ?? "Premier Employee";
            var record = new RecognitionRecord
            {
                Id = Guid.NewGuid().ToString(),
                GiverName = giverName,
                ReceiverName = request.ReceiverName,
                ReceiverEmail = request.ReceiverEmail,
                Category = request.Category,
                Message = request.Message,
                Points = request.Points,
                AwardedDate = DateTime.UtcNow
            };

            await _repository.SendRecognitionAsync(record);

            return new RecognitionRecordDto
            {
                Id = record.Id,
                GiverName = record.GiverName,
                ReceiverName = record.ReceiverName,
                ReceiverEmail = record.ReceiverEmail,
                Category = record.Category,
                Message = record.Message,
                Points = record.Points,
                AwardedDate = record.AwardedDate
            };
        }
    }
}
