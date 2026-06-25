using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AttendanceFeature.Application.Queries;
using AttendanceFeature.Application.DTO;

namespace AttendanceFeature.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class AttendanceQueries
    {
        [Authorize]
        public async Task<AttendanceRecordDto?> GetMyTodayAttendanceAsync([Service] IMediator mediator, [Service] IHttpContextAccessor httpContextAccessor)
        {
            var userId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            return await mediator.Send(new GetMyTodayAttendanceQuery { UserId = userId });
        }

        [Authorize]
        public async Task<List<AttendanceRecordDto>> GetMyAttendanceAsync(
            [Service] IMediator mediator, 
            [Service] IHttpContextAccessor httpContextAccessor,
            int month, 
            int year)
        {
            var userId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            return await mediator.Send(new GetMyAttendanceQuery { UserId = userId, Input = new GetMyAttendanceInput { Month = month, Year = year } });
        }

        [Authorize]
        public async Task<List<AttendanceRecordDto>> GetTeamAttendanceAsync(
            [Service] IMediator mediator,
            [Service] IHttpContextAccessor httpContextAccessor,
            System.DateTime date)
        {
            var hasPermission = httpContextAccessor.HttpContext?.User?.IsInRole("Manager") == true ||
                                httpContextAccessor.HttpContext?.User?.IsInRole("ReportingManager") == true ||
                                httpContextAccessor.HttpContext?.User?.IsInRole("HR") == true ||
                                httpContextAccessor.HttpContext?.User?.IsInRole("Admin") == true;

            return await mediator.Send(new GetTeamAttendanceQuery { HasPermission = hasPermission, Input = new GetTeamAttendanceInput { Date = date } });
        }
    }
}
