using ContributionsFeature.Application.DTO;
using ContributionsFeature.Application.Repository;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ContributionsFeature.Application.Commands
{
    public class ApproveContributionCommand : IRequest<ValueContributionDto>
    {
        public string ContributionId { get; set; } = string.Empty;
        public int FinalPoints { get; set; }
        public string Comments { get; set; } = string.Empty;
    }

    public class ApproveContributionCommandHandler : IRequestHandler<ApproveContributionCommand, ValueContributionDto>
    {
        private readonly IContributionRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApproveContributionCommandHandler(IContributionRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ValueContributionDto> Handle(ApproveContributionCommand request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var approverName = user?.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value 
                ?? user?.FindFirst("name")?.Value 
                ?? "Michael (Manager)";
            var x = await _repository.ApproveContributionAsync(request.ContributionId, request.FinalPoints, request.Comments, approverName);

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
