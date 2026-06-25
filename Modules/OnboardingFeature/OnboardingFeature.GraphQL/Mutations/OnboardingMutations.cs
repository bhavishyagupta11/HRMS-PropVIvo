using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using Microsoft.AspNetCore.Http;
using OnboardingFeature.Application.Commands;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace OnboardingFeature.GraphQL.Mutations
{
    [ExtendObjectType("Mutation")]
    public class OnboardingMutations
    {
        // ON-02: "I can mark employee-assigned tasks as complete."
        [Authorize]
        public async Task<bool> CompleteOnboardingTaskAsync(
            string taskId,
            [Service] IMediator mediator,
            [Service] IHttpContextAccessor httpContextAccessor,
            CancellationToken cancellationToken)
        {
            var userId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            return await mediator.Send(new CompleteOnboardingTaskCommand
            {
                TaskId = taskId,
                UserId = userId
            }, cancellationToken);
        }

        // ON-06: "I want to mark my onboarding complete so that I transition into the standard employee experience."
        [Authorize]
        public async Task<bool> CompleteOnboardingAsync(
            [Service] IMediator mediator,
            [Service] IHttpContextAccessor httpContextAccessor,
            CancellationToken cancellationToken)
        {
            var userId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            return await mediator.Send(new CompleteOnboardingCommand { UserId = userId }, cancellationToken);
        }
    }
}
