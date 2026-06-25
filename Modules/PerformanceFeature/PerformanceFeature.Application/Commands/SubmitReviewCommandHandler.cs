using MediatR;
using Microsoft.AspNetCore.Http;
using PerformanceFeature.Application.DTO;
using PerformanceFeature.Application.Repository;
using PerformanceFeature.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PerformanceFeature.Application.Commands
{
    public class SubmitReviewCommand : IRequest<PerformanceReviewRecordDto>
    {
        public string ReviewCycle { get; set; } = string.Empty;
        public decimal SelfRating { get; set; }
        public decimal ManagerRating { get; set; }
        public string Comments { get; set; } = string.Empty;
        public string ReviewerUserId { get; set; } = string.Empty;
    }

    public class SubmitReviewCommandHandler : IRequestHandler<SubmitReviewCommand, PerformanceReviewRecordDto>
    {
        private readonly IPerformanceRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SubmitReviewCommandHandler(IPerformanceRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PerformanceReviewRecordDto> Handle(SubmitReviewCommand request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
                ?? user?.FindFirst("sub")?.Value 
                ?? "USR-9988231-HRMS";
            var record = new PerformanceReviewRecord
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                ReviewCycle = request.ReviewCycle,
                SelfRating = request.SelfRating,
                ManagerRating = request.ManagerRating,
                Comments = request.Comments,
                ReviewerUserId = request.ReviewerUserId
            };

            await _repository.CreateReviewAsync(record);

            return new PerformanceReviewRecordDto
            {
                Id = record.Id,
                UserId = record.UserId,
                ReviewCycle = record.ReviewCycle,
                SelfRating = record.SelfRating,
                ManagerRating = record.ManagerRating,
                Comments = record.Comments,
                ReviewerUserId = record.ReviewerUserId
            };
        }
    }
}
