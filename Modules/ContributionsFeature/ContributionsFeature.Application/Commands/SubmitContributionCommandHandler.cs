using ContributionsFeature.Application.DTO;
using ContributionsFeature.Application.Repository;
using ContributionsFeature.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ContributionsFeature.Application.Commands
{
    public class SubmitContributionCommand : IRequest<ValueContributionDto>
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ContributionType { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int SuggestedPoints { get; set; }
        public string ImpactLevel { get; set; } = string.Empty;
    }

    public class SubmitContributionCommandHandler : IRequestHandler<SubmitContributionCommand, ValueContributionDto>
    {
        private readonly IContributionRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SubmitContributionCommandHandler(IContributionRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ValueContributionDto> Handle(SubmitContributionCommand request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var employeeName = user?.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value 
                ?? user?.FindFirst("name")?.Value 
                ?? "Sarah (Employee)";
            var contribution = new ValueContribution
            {
                Id = Guid.NewGuid().ToString(),
                Title = request.Title,
                Description = request.Description,
                ContributionType = request.ContributionType,
                Category = request.Category,
                Status = "under-review",
                Points = 0,
                SuggestedPoints = request.SuggestedPoints,
                ImpactLevel = request.ImpactLevel,
                EmployeeName = employeeName,
                SubmittedDate = DateTime.UtcNow
            };

            var x = await _repository.SubmitContributionAsync(contribution);

            return new ValueContributionDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                ContributionType = x.ContributionType,
                Category = x.Category,
                Status = x.Status,
                Points = x.Points,
                SuggestedPoints = x.SuggestedPoints,
                ImpactLevel = x.ImpactLevel,
                EmployeeName = x.EmployeeName,
                ApproverName = x.ApproverName,
                ApprovalComments = x.ApprovalComments,
                SubmittedDate = x.SubmittedDate,
                ApprovedDate = x.ApprovedDate
            };
        }
    }
}
