using CopilotFeature.Application.DTO;
using CopilotFeature.Application.Repository;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CopilotFeature.Application.Queries
{
    public class GetInteractionHistoryQuery : IRequest<IEnumerable<CopilotInteractionRecordDto>>
    {
    }

    public class GetInteractionHistoryQueryHandler : IRequestHandler<GetInteractionHistoryQuery, IEnumerable<CopilotInteractionRecordDto>>
    {
        private readonly ICopilotRepository _repository;

        public GetInteractionHistoryQueryHandler(ICopilotRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CopilotInteractionRecordDto>> Handle(GetInteractionHistoryQuery request, CancellationToken cancellationToken)
        {
            var records = await _repository.GetInteractionHistoryAsync();
            return records.Select(x => new CopilotInteractionRecordDto
            {
                Id = x.Id,
                PromptText = x.PromptText,
                ResponseText = x.ResponseText,
                Category = x.Category,
                ConfidenceScore = x.ConfidenceScore,
                InteractionDate = x.InteractionDate
            });
        }
    }
}
