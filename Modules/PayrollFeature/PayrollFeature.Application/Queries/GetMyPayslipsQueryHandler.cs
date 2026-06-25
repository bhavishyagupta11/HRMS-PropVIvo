using Microsoft.AspNetCore.Http;
using MediatR;
using PayrollFeature.Application.DTO;
using PayrollFeature.Application.Repository;
using PayrollFeature.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PayrollFeature.Application.Queries
{
    public class GetMyPayslipsQuery : IRequest<IEnumerable<PayrollRecordDto>>
    {
    }

    public class GetMyPayslipsQueryHandler : IRequestHandler<GetMyPayslipsQuery, IEnumerable<PayrollRecordDto>>
    {
        private readonly IPayrollRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMyPayslipsQueryHandler(IPayrollRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<PayrollRecordDto>> Handle(GetMyPayslipsQuery request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                ?? user?.Claims?.FirstOrDefault(c => c.Type == "sub")?.Value
                ?? "";
            
            var recordsList = (await _repository.GetPayslipsAsync(userId)).ToList();

            if (!recordsList.Any())
            {
                var sampleSlips = new[]
                {
                    new { Period = "June 2026", Date = new DateTime(2026, 6, 30, 0, 0, 0, DateTimeKind.Utc), Country = "IN", Gross = 84000m, Ded = 11340m, Net = 72660m,
                          Earnings = "[{\"label\":\"Basic Salary\",\"amount\":45000},{\"label\":\"House Rent Allowance (HRA)\",\"amount\":18000},{\"label\":\"Special Allowance\",\"amount\":12000},{\"label\":\"Performance Bonus\",\"amount\":5000},{\"label\":\"Overtime Pay\",\"amount\":2500},{\"label\":\"Reimbursements\",\"amount\":1500}]",
                          Deductions = "[{\"label\":\"Provident Fund (PF)\",\"amount\":5400},{\"label\":\"Income Tax (TDS)\",\"amount\":4200},{\"label\":\"Professional Tax\",\"amount\":200},{\"label\":\"Employee State Insurance (ESI)\",\"amount\":315},{\"label\":\"Health Insurance\",\"amount\":1200},{\"label\":\"Labour Welfare Fund (LWF)\",\"amount\":25}]",
                          Contributions = "[{\"label\":\"Employer PF Contribution\",\"amount\":5400},{\"label\":\"Employer ESI Contribution\",\"amount\":1470},{\"label\":\"Gratuity\",\"amount\":2165}]" },
                    new { Period = "May 2026", Date = new DateTime(2026, 5, 31, 0, 0, 0, DateTimeKind.Utc), Country = "IN", Gross = 82500m, Ded = 11180m, Net = 71320m,
                          Earnings = "[{\"label\":\"Basic Salary\",\"amount\":45000},{\"label\":\"House Rent Allowance (HRA)\",\"amount\":18000},{\"label\":\"Special Allowance\",\"amount\":12000},{\"label\":\"Overtime Pay\",\"amount\":6000},{\"label\":\"Reimbursements\",\"amount\":1500}]",
                          Deductions = "[{\"label\":\"Provident Fund (PF)\",\"amount\":5400},{\"label\":\"Income Tax (TDS)\",\"amount\":4200},{\"label\":\"Professional Tax\",\"amount\":200},{\"label\":\"Health Insurance\",\"amount\":1380}]",
                          Contributions = "[{\"label\":\"Employer PF Contribution\",\"amount\":5400},{\"label\":\"Gratuity\",\"amount\":2165}]" },
                    new { Period = "June 2026", Date = new DateTime(2026, 6, 30, 0, 0, 0, DateTimeKind.Utc), Country = "US", Gross = 7500m, Ded = 2047.25m, Net = 5452.75m,
                          Earnings = "[{\"label\":\"Basic Salary\",\"amount\":5500},{\"label\":\"Housing Allowance\",\"amount\":1000},{\"label\":\"Overtime Pay\",\"amount\":600},{\"label\":\"Performance Bonus\",\"amount\":400}]",
                          Deductions = "[{\"label\":\"Federal Income Tax (FIT)\",\"amount\":1050},{\"label\":\"Social Security Tax (FICA)\",\"amount\":403},{\"label\":\"Medicare Tax (FICA)\",\"amount\":94.25},{\"label\":\"401(k) Pre-tax Contribution\",\"amount\":350},{\"label\":\"Health Insurance Premium\",\"amount\":150}]",
                          Contributions = "[{\"label\":\"Employer FICA (SS & Med)\",\"amount\":497.25},{\"label\":\"FUTA (Federal Unemployment)\",\"amount\":42},{\"label\":\"401(k) Match Contribution\",\"amount\":175}]" }
                };

                foreach (var slip in sampleSlips)
                {
                    var record = new PayrollRecord
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = userId,
                        PayPeriod = slip.Period,
                        PayDate = slip.Date,
                        CountryCode = slip.Country,
                        GrossPay = slip.Gross,
                        TotalDeductions = slip.Ded,
                        NetPay = slip.Net,
                        EarningsJson = slip.Earnings,
                        DeductionsJson = slip.Deductions,
                        EmployerContributionsJson = slip.Contributions,
                        Status = "Paid",
                        CreatedByUserId = "System",
                        CreatedByUserName = "System",
                        CreatedOn = DateTime.UtcNow
                    };
                    await _repository.CreatePayslipAsync(record);
                    recordsList.Add(record);
                }
            }

            return recordsList.Select(record => new PayrollRecordDto
            {
                Id = record.Id,
                UserId = record.UserId,
                PayPeriod = record.PayPeriod,
                PayDate = record.PayDate,
                CountryCode = record.CountryCode,
                GrossPay = record.GrossPay,
                TotalDeductions = record.TotalDeductions,
                NetPay = record.NetPay,
                EarningsJson = record.EarningsJson,
                DeductionsJson = record.DeductionsJson,
                EmployerContributionsJson = record.EmployerContributionsJson,
                Status = record.Status
            });
        }
    }
}
