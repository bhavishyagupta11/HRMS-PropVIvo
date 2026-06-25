using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using AttendanceFeature.Application.DTO;
using AttendanceFeature.Domain;
using AttendanceFeature.Domain.Repositories;

namespace AttendanceFeature.Application.Commands
{
    public class ClockInCommand : IRequest<bool>
    {
        public string UserId { get; set; } = string.Empty;
        public ClockInInput Input { get; set; } = new();
    }

    public class ClockInCommandHandler : IRequestHandler<ClockInCommand, bool>
    {
        private readonly IAttendanceRepository _repository;

        public ClockInCommandHandler(IAttendanceRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(ClockInCommand request, CancellationToken cancellationToken)
        {
            var today = DateTime.UtcNow.Date;

            var existingRecord = await _repository.GetTodayRecordAsync(request.UserId, today);
            if (existingRecord != null)
            {
                throw new InvalidOperationException("Already clocked in today.");
            }

            var record = new AttendanceRecord
            {
                Id = Guid.NewGuid().ToString(),
                UserId = request.UserId,
                Date = today,
                ClockInTime = DateTime.UtcNow,
                ClockInMethod = request.Input.ClockInMethod,
                LocationVerified = request.Input.LocationVerified,
                IpValidated = request.Input.IpValidated,
                SelfieUrl = request.Input.SelfieUrl,
                ShiftName = request.Input.ShiftName,
                ShiftStartTime = request.Input.ShiftStartTime,
                ShiftEndTime = request.Input.ShiftEndTime,
                Status = "Present"
            };

            await _repository.AddAsync(record);
            return true;
        }
    }
}
