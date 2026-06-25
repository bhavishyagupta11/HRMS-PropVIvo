using HRMS.Core.Postgres.Common;
using HRMS.Shared.Domain.Entity;
using System;

namespace AttendanceFeature.Domain
{
    public class AttendanceRecord : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public DateTime ClockInTime { get; set; }
        public DateTime? ClockOutTime { get; set; }
        
        // Method and Verification (Deferred actual verification logic, just marking state)
        public string ClockInMethod { get; set; } = string.Empty;
        public bool LocationVerified { get; set; }
        public bool IpValidated { get; set; }
        public string? SelfieUrl { get; set; }

        // Computed Hours
        public double TotalHours { get; set; }
        public double ProductiveHours { get; set; }
        public double BreakHours { get; set; }
        public double OvertimeHours { get; set; }

        // Status: "Present", "Absent", "Late", "Half-Day", "On-Leave"
        public string Status { get; set; } = string.Empty;

        // Shift Details (Denormalized)
        public string ShiftName { get; set; } = string.Empty;
        public TimeSpan ShiftStartTime { get; set; }
        public TimeSpan ShiftEndTime { get; set; }
        public UserBase? UserContext { get; set; }
    }
}
