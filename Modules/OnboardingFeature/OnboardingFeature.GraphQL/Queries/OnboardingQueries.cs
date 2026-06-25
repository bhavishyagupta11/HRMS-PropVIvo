using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using Microsoft.AspNetCore.Http;
using OnboardingFeature.Application.DTO;
using OnboardingFeature.Application.Queries;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace OnboardingFeature.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class OnboardingQueries
    {
        // ON-01: Personalized onboarding dashboard
        [Authorize]
        public async Task<OnboardingEmployeeDto?> MyOnboardingAsync(
            [Service] IMediator mediator,
            [Service] IHttpContextAccessor httpContextAccessor,
            CancellationToken cancellationToken)
        {
            var userId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            return await mediator.Send(new GetMyOnboardingQuery { UserId = userId }, cancellationToken);
        }

        // ON-01/02: Task list grouped by phase — optional phase filter
        [Authorize]
        public async Task<List<OnboardingTaskDto>> MyOnboardingTasksAsync(
            string onboardingEmployeeId,
            string? phase,
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetMyOnboardingTasksQuery
            {
                OnboardingEmployeeId = onboardingEmployeeId,
                Phase = phase
            }, cancellationToken);
        }

        // ON-03: Welcome messages with optional video
        [Authorize]
        public async Task<List<WelcomeMessageDto>> MyWelcomeMessagesAsync(
            string onboardingEmployeeId,
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetMyWelcomeMessagesQuery
            {
                OnboardingEmployeeId = onboardingEmployeeId
            }, cancellationToken);
        }

        // ON-04: Relocation support details
        [Authorize]
        public async Task<RelocationSupportDto?> MyRelocationSupportAsync(
            string onboardingEmployeeId,
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetMyRelocationSupportQuery
            {
                OnboardingEmployeeId = onboardingEmployeeId
            }, cancellationToken);
        }

        // ON-05: Team introductions
        [Authorize]
        public async Task<List<TeamIntroductionDto>> MyTeamIntroductionsAsync(
            string onboardingEmployeeId,
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetMyTeamIntroductionsQuery
            {
                OnboardingEmployeeId = onboardingEmployeeId
            }, cancellationToken);
        }

        // PSD Key Capabilities: milestones with scheduled dates
        [Authorize]
        public async Task<List<OnboardingMilestoneDto>> MyOnboardingMilestonesAsync(
            string onboardingEmployeeId,
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetMyOnboardingMilestonesQuery
            {
                OnboardingEmployeeId = onboardingEmployeeId
            }, cancellationToken);
        }

        // PSD Key Capabilities: training modules with progress and certificates
        [Authorize]
        public async Task<List<OnboardingTrainingModuleRefDto>> MyOnboardingTrainingModulesAsync(
            string onboardingEmployeeId,
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetMyOnboardingTrainingModulesQuery
            {
                OnboardingEmployeeId = onboardingEmployeeId
            }, cancellationToken);
        }
    }
}
