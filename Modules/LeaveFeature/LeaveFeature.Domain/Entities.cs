using HRMS.Core.Postgres.Common;
using HRMS.Shared.Domain.Entity;
using System;
using System.Collections.Generic;

namespace LeaveFeature.Domain
{
    public class LeaveBalance : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public string LeaveType { get; set; } = string.Empty;
        public int TotalAllowed { get; set; }
        public int Used { get; set; }
        public int Pending { get; set; }
        public int Available { get; set; }
        public int CarriedForward { get; set; }
        public int Encashed { get; set; }
    }

    public class LeaveApproval : BaseEntity
    {
        public string LeaveRequestId { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty; // "Manager", "ReportingManager", "HR"
        public string Status { get; set; } = "Pending"; // "Pending", "Approved", "Rejected", "Cancelled"
        public string? ApproverId { get; set; }
        public string? Comment { get; set; }
        public DateTime? ActionedAt { get; set; }
    }

    public class LeaveRequest : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public string LeaveType { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalDays { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected, Cancelled
        public string CurrentApprovalLevel { get; set; } = "Manager"; // "Manager", "ReportingManager", "HR", "Completed"
        public string? ApproverId { get; set; }
        public string? ApproverComments { get; set; }

        // Navigation property for approvals history
        public List<LeaveApproval> Approvals { get; set; } = new();
    }
}
