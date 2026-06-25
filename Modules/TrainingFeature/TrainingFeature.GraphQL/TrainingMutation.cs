using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using System.Threading.Tasks;
using TrainingFeature.Application.Commands;
using TrainingFeature.Application.DTO;

namespace TrainingFeature.GraphQL
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class TrainingMutation
    {
        [Authorize]
        public async Task<CourseEnrollmentRecordDto> EnrollInCourse(EnrollInCourseCommand command, [Service] IMediator mediator)
        {
            return await mediator.Send(command);
        }

        [Authorize]
        public async Task<CourseEnrollmentRecordDto> CompleteCourse(CompleteCourseCommand command, [Service] IMediator mediator)
        {
            return await mediator.Send(command);
        }
    }
}
