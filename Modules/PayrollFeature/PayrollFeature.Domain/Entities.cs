using HRMS.Core.Postgres.Common;
using System;

namespace PayrollFeature.Domain
{
    public class PayrollRecord : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public string PayPeriod { get; set; } = string.Empty;
        public DateTime PayDate { get; set; }
        public string CountryCode { get; set; } = string.Empty;
        
        // Draft, Processing, Approved, Paid
        public string Status { get; set; } = string.Empty;
        
        public decimal GrossPay { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetPay { get; set; }
        
        public string EarningsJson { get; set; } = string.Empty;
        public string DeductionsJson { get; set; } = string.Empty;
        public string EmployerContributionsJson { get; set; } = string.Empty;
    }

    public class ComplianceDocument : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public string ComplianceDocType { get; set; } = string.Empty;
        public string DocumentUrl { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public int Year { get; set; }
    }
}
