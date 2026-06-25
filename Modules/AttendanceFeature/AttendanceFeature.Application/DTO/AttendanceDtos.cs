using System;

namespace AttendanceFeature.Application.DTO
{
    public class AttendanceRecordDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public DateTime ClockInTime { get; set; }
        public DateTime? ClockOutTime { get; set; }
        public string ClockInMethod { get; set; } = string.Empty;
        public bool LocationVerified { get; set; }
        public bool IpValidated { get; set; }
        public string? SelfieUrl { get; set; }
        public double TotalHours { get; set; }
        public double ProductiveHours { get; set; }
        public double BreakHours { get; set; }
        public double OvertimeHours { get; set; }
        public string Status { get; set; } = string.Empty;
        public string ShiftName { get; set; } = string.Empty;
        public TimeSpan ShiftStartTime { get; set; }
        public TimeSpan ShiftEndTime { get; set; }
    }

    public class ClockInInput
    {
        public string ClockInMethod { get; set; } = string.Empty;
        public string ShiftName { get; set; } = string.Empty;
        public TimeSpan ShiftStartTime { get; set; }
        public TimeSpan ShiftEndTime { get; set; }
        public bool LocationVerified { get; set; }
        public bool IpValidated { get; set; }
        public string? SelfieUrl { get; set; }
    }

    public class GetMyAttendanceInput
    {
        public int Month { get; set; }
        public int Year { get; set; }
    }

    public class GetTeamAttendanceInput
    {
        public DateTime Date { get; set; }
    }
}
