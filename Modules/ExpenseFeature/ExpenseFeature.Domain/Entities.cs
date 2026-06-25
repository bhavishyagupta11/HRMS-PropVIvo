using HRMS.Shared.Domain.Entity;
using HRMS.Core.Postgres.Common;
using System;

namespace ExpenseFeature.Domain
{
    public class ReimbursementRecord : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // travel, food, accommodation, communication, medical, office-supplies, other
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public string Description { get; set; } = string.Empty;
        public DateTime ExpenseDate { get; set; }
        public string? ReceiptUrl { get; set; }
        public decimal? Mileage { get; set; } // distance in km
        public bool IsTaxable { get; set; }
        public string Status { get; set; } = "Submitted"; // draft, submitted, pending-approval, approved, rejected, paid
        public string? ApprovalComments { get; set; }
        public DateTime? PaidDate { get; set; }
    }
}
