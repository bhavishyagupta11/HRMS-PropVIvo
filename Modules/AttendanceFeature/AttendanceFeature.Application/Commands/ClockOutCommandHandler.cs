using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using AttendanceFeature.Domain.Repositories;

namespace AttendanceFeature.Application.Commands
{
    public class ClockOutCommand : IRequest<bool>
    {
        public string UserId { get; set; } = string.Empty;
    }

    public class ClockOutCommandHandler : IRequestHandler<ClockOutCommand, bool>
    {
        private readonly IAttendanceRepository _repository;

        public ClockOutCommandHandler(IAttendanceRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(ClockOutCommand request, CancellationToken cancellationToken)
        {
            var today = DateTime.UtcNow.Date;

            var existingRecord = await _repository.GetTodayRecordAsync(request.UserId, today);
            if (existingRecord == null)
            {
                throw new InvalidOperationException("Not clocked in today.");
            }
            if (existingRecord.ClockOutTime.HasValue)
            {
                throw new InvalidOperationException("Already clocked out today.");
            }

            existingRecord.ClockOutTime = DateTime.UtcNow;
            
            var duration = existingRecord.ClockOutTime.Value - existingRecord.ClockInTime;
            existingRecord.TotalHours = duration.TotalHours;
            
            var shiftDuration = existingRecord.ShiftEndTime - existingRecord.ShiftStartTime;
            double standardHours = shiftDuration.TotalHours > 0 ? shiftDuration.TotalHours : 8.0;

            if (existingRecord.TotalHours > standardHours)
            {
                existingRecord.ProductiveHours = standardHours;
                existingRecord.OvertimeHours = existingRecord.TotalHours - standardHours;
            }
            else
            {
                existingRecord.ProductiveHours = existingRecord.TotalHours;
                existingRecord.OvertimeHours = 0;
            }

            if (existingRecord.TotalHours > 4)
            {
                existingRecord.BreakHours = 1.0;
            }

            await _repository.UpdateAsync(existingRecord);
            return true;
        }
    }
}
