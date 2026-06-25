using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using AttendanceFeature.Application.DTO;
using AttendanceFeature.Domain.Repositories;

namespace AttendanceFeature.Application.Queries
{
    public class GetMyTodayAttendanceQuery : IRequest<AttendanceRecordDto?>
    {
        public string UserId { get; set; } = string.Empty;
    }

    public class GetMyTodayAttendanceQueryHandler : IRequestHandler<GetMyTodayAttendanceQuery, AttendanceRecordDto?>
    {
        private readonly IAttendanceRepository _repository;

        public GetMyTodayAttendanceQueryHandler(IAttendanceRepository repository)
        {
            _repository = repository;
        }

        public async Task<AttendanceRecordDto?> Handle(GetMyTodayAttendanceQuery request, CancellationToken cancellationToken)
        {
            var record = await _repository.GetTodayRecordAsync(request.UserId, DateTime.UtcNow.Date);
            if (record == null) return null;

            return new AttendanceRecordDto
            {
                Id = record.Id,
                UserId = record.UserId,
                Date = record.Date,
                ClockInTime = record.ClockInTime,
                ClockOutTime = record.ClockOutTime,
                ClockInMethod = record.ClockInMethod,
                LocationVerified = record.LocationVerified,
                IpValidated = record.IpValidated,
                SelfieUrl = record.SelfieUrl,
                TotalHours = record.TotalHours,
                ProductiveHours = record.ProductiveHours,
                BreakHours = record.BreakHours,
                OvertimeHours = record.OvertimeHours,
                Status = record.Status,
                ShiftName = record.ShiftName,
                ShiftStartTime = record.ShiftStartTime,
                ShiftEndTime = record.ShiftEndTime
            };
        }
    }
}
