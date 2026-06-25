using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AttendanceFeature.Application.DTO;
using AttendanceFeature.Domain.Repositories;

namespace AttendanceFeature.Application.Queries
{
    public class GetTeamAttendanceQuery : IRequest<List<AttendanceRecordDto>>
    {
        public bool HasPermission { get; set; }
        public GetTeamAttendanceInput Input { get; set; } = new();
    }

    public class GetTeamAttendanceQueryHandler : IRequestHandler<GetTeamAttendanceQuery, List<AttendanceRecordDto>>
    {
        private readonly IAttendanceRepository _repository;

        public GetTeamAttendanceQueryHandler(IAttendanceRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<AttendanceRecordDto>> Handle(GetTeamAttendanceQuery request, CancellationToken cancellationToken)
        {
            if (!request.HasPermission)
            {
                throw new UnauthorizedAccessException("You do not have permission to view team attendance.");
            }

            var records = await _repository.GetTeamRecordsAsync(request.Input.Date.Date);
            return records.Select(r => new AttendanceRecordDto
            {
                Id = r.Id,
                UserId = r.UserId,
                Date = r.Date,
                ClockInTime = r.ClockInTime,
                ClockOutTime = r.ClockOutTime,
                ClockInMethod = r.ClockInMethod,
                LocationVerified = r.LocationVerified,
                IpValidated = r.IpValidated,
                SelfieUrl = r.SelfieUrl,
                TotalHours = r.TotalHours,
                ProductiveHours = r.ProductiveHours,
                BreakHours = r.BreakHours,
                OvertimeHours = r.OvertimeHours,
                Status = r.Status,
                ShiftName = r.ShiftName,
                ShiftStartTime = r.ShiftStartTime,
                ShiftEndTime = r.ShiftEndTime
            }).ToList();
        }
    }
}
