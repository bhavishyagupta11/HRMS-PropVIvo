using MediatR;
using RecognitionFeature.Application.DTO;
using RecognitionFeature.Application.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RecognitionFeature.Application.Queries
{
    public class GetRecentRecognitionsQuery : IRequest<IEnumerable<RecognitionRecordDto>>
    {
    }

    public class GetRecentRecognitionsQueryHandler : IRequestHandler<GetRecentRecognitionsQuery, IEnumerable<RecognitionRecordDto>>
    {
        private readonly IRecognitionRepository _repository;

        public GetRecentRecognitionsQueryHandler(IRecognitionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<RecognitionRecordDto>> Handle(GetRecentRecognitionsQuery request, CancellationToken cancellationToken)
        {
            var records = await _repository.GetRecentRecognitionsAsync();
            return records.Select(x => new RecognitionRecordDto
            {
                Id = x.Id,
                GiverName = x.GiverName,
                ReceiverName = x.ReceiverName,
                ReceiverEmail = x.ReceiverEmail,
                Category = x.Category,
                Message = x.Message,
                Points = x.Points,
                AwardedDate = x.AwardedDate
            });
        }
    }
}
