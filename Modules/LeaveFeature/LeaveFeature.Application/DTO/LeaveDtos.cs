using System;
using System.Collections.Generic;

namespace LeaveFeature.Application.DTO
{
    public class LeaveBalanceDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string LeaveType { get; set; } = string.Empty;
        public int TotalAllowed { get; set; }
        public int Used { get; set; }
        public int Pending { get; set; }
        public int Available { get; set; }
        public int CarriedForward { get; set; }
        public int Encashed { get; set; }
    }

    public class LeaveApprovalDto
    {
        public string Id { get; set; } = string.Empty;
        public string LeaveRequestId { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty; // "Manager", "ReportingManager", "HR"
        public string Status { get; set; } = string.Empty; // "Pending", "Approved", "Rejected", "Cancelled"
        public string? ApproverId { get; set; }
        public string? Comment { get; set; }
        public DateTime? ActionedAt { get; set; }
    }

    public class LeaveRequestDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public string LeaveType { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalDays { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? ApproverId { get; set; }
        public string? ApproverComments { get; set; }
        public string CurrentApprovalLevel { get; set; } = string.Empty;
        public List<LeaveApprovalDto> Approvals { get; set; } = new();
    }

    public class SubmitLeaveRequestDto
    {
        public string LeaveType { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalDays { get; set; }
        public string Reason { get; set; } = string.Empty;
    }

    public class ProcessLeaveRequestDto
    {
        public string RequestId { get; set; } = string.Empty;
        public bool Approve { get; set; }
        public string? Comments { get; set; }
    }
}
