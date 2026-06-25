using System;

namespace ExpenseFeature.Application.DTO
{
    public class ReimbursementRecordDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime ExpenseDate { get; set; }
        public string? ReceiptUrl { get; set; }
        public decimal? Mileage { get; set; }
        public bool IsTaxable { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? ApprovalComments { get; set; }
        public DateTime? PaidDate { get; set; }
    }

    public class SubmitExpenseDto
    {
        public string Category { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public string Description { get; set; } = string.Empty;
        public DateTime ExpenseDate { get; set; }
        public string? ReceiptFileName { get; set; }
        public string? ReceiptFileType { get; set; }
        public string? ReceiptBase64Content { get; set; }
        public decimal? Mileage { get; set; }
        public bool IsTaxable { get; set; }
    }

    public class ProcessExpenseDto
    {
        public string ExpenseId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; // approved, rejected, paid
        public string? ApprovalComments { get; set; }
    }
}
