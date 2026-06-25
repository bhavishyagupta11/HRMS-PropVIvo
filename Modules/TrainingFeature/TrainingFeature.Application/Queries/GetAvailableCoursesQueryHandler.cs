using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrainingFeature.Application.DTO;
using TrainingFeature.Application.Repository;

namespace TrainingFeature.Application.Queries
{
    public class GetAvailableCoursesQuery : IRequest<IEnumerable<TrainingCourseRecordDto>>
    {
    }

    public class GetAvailableCoursesQueryHandler : IRequestHandler<GetAvailableCoursesQuery, IEnumerable<TrainingCourseRecordDto>>
    {
        private readonly ITrainingRepository _repository;

        public GetAvailableCoursesQueryHandler(ITrainingRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TrainingCourseRecordDto>> Handle(GetAvailableCoursesQuery request, CancellationToken cancellationToken)
        {
            var records = await _repository.GetAvailableCoursesAsync();
            return records.Select(x => new TrainingCourseRecordDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                TrainerName = x.TrainerName,
                Credits = x.Credits,
                ScheduleDate = x.ScheduleDate
            });
        }
    }
}
