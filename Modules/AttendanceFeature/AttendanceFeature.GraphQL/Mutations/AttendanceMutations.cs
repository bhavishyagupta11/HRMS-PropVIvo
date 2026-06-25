using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using AttendanceFeature.Application.Commands;
using AttendanceFeature.Application.DTO;

namespace AttendanceFeature.GraphQL.Mutations
{
    [ExtendObjectType("Mutation")]
    public class AttendanceMutations
    {
        [Authorize]
        public async Task<bool> ClockInAsync(
            [Service] IMediator mediator,
            [Service] IHttpContextAccessor httpContextAccessor,
            string clockInMethod,
            string shiftName,
            string shiftStartTime,
            string shiftEndTime,
            bool locationVerified,
            bool ipValidated,
            string? selfieUrl = null)
        {
            var userId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            var command = new ClockInCommand
            {
                UserId = userId,
                Input = new ClockInInput
                {
                    ClockInMethod = clockInMethod,
                    ShiftName = shiftName,
                    ShiftStartTime = System.TimeSpan.Parse(shiftStartTime),
                    ShiftEndTime = System.TimeSpan.Parse(shiftEndTime),
                    LocationVerified = locationVerified,
                    IpValidated = ipValidated,
                    SelfieUrl = selfieUrl
                }
            };
            return await mediator.Send(command);
        }

        [Authorize]
        public async Task<bool> ClockOutAsync([Service] IMediator mediator, [Service] IHttpContextAccessor httpContextAccessor)
        {
            var userId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            return await mediator.Send(new ClockOutCommand { UserId = userId });
        }
    }
}
