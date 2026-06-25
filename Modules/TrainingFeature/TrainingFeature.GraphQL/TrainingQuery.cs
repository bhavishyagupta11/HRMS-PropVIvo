using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrainingFeature.Application.DTO;
using TrainingFeature.Application.Queries;

namespace TrainingFeature.GraphQL
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class TrainingQuery
    {
        [Authorize]
        public async Task<IEnumerable<TrainingCourseRecordDto>> GetAvailableCourses([Service] IMediator mediator)
        {
            return await mediator.Send(new GetAvailableCoursesQuery());
        }

        [Authorize]
        public async Task<IEnumerable<CourseEnrollmentRecordDto>> GetMyEnrollments([Service] IMediator mediator)
        {
            return await mediator.Send(new GetMyEnrollmentsQuery());
        }
    }
}
