using System;

namespace PayrollFeature.Application.DTO
{
    public class PayrollRecordDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string PayPeriod { get; set; } = string.Empty;
        public DateTime PayDate { get; set; }
        public string CountryCode { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal GrossPay { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetPay { get; set; }
        public string EarningsJson { get; set; } = string.Empty;
        public string DeductionsJson { get; set; } = string.Empty;
        public string EmployerContributionsJson { get; set; } = string.Empty;
    }

    public class GeneratePayslipDto
    {
        public string UserId { get; set; } = string.Empty;
        public string PayPeriod { get; set; } = string.Empty;
        public DateTime PayDate { get; set; }
        public string CountryCode { get; set; } = string.Empty;
        public decimal GrossPay { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetPay { get; set; }
        public string EarningsJson { get; set; } = string.Empty;
        public string DeductionsJson { get; set; } = string.Empty;
        public string EmployerContributionsJson { get; set; } = string.Empty;
    }
}
