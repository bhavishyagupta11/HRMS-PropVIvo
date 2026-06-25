using ContributionsFeature.Application.DTO;
using ContributionsFeature.Application.Repository;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ContributionsFeature.Application.Commands
{
    public class ClaimContributionItemCommand : IRequest<ContributionItemDto>
    {
        public string ItemId { get; set; } = string.Empty;
    }

    public class ClaimContributionItemCommandHandler : IRequestHandler<ClaimContributionItemCommand, ContributionItemDto>
    {
        private readonly IContributionRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimContributionItemCommandHandler(IContributionRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ContributionItemDto> Handle(ClaimContributionItemCommand request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var employeeName = user?.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value 
                ?? user?.FindFirst("name")?.Value 
                ?? "Sarah (Employee)";
            var item = await _repository.ClaimContributionItemAsync(request.ItemId, employeeName);

            return new ContributionItemDto
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                Category = item.Category,
                SuggestedPoints = item.SuggestedPoints,
                Status = item.Status,
                ClaimedByEmployee = item.ClaimedByEmployee
            };
        }
    }
}
