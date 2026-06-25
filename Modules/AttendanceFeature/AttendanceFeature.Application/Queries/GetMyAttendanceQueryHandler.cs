using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AttendanceFeature.Application.DTO;
using AttendanceFeature.Domain.Repositories;

namespace AttendanceFeature.Application.Queries
{
    public class GetMyAttendanceQuery : IRequest<List<AttendanceRecordDto>>
    {
        public string UserId { get; set; } = string.Empty;
        public GetMyAttendanceInput Input { get; set; } = new();
    }

    public class GetMyAttendanceQueryHandler : IRequestHandler<GetMyAttendanceQuery, List<AttendanceRecordDto>>
    {
        private readonly IAttendanceRepository _repository;

        public GetMyAttendanceQueryHandler(IAttendanceRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<AttendanceRecordDto>> Handle(GetMyAttendanceQuery request, CancellationToken cancellationToken)
        {
            var records = await _repository.GetMonthlyRecordsAsync(request.UserId, request.Input.Month, request.Input.Year);
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
