using Microsoft.AspNetCore.Http;
using MediatR;
using PayrollFeature.Application.DTO;
using PayrollFeature.Application.Repository;
using PayrollFeature.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PayrollFeature.Application.Commands
{
    public class GeneratePayslipCommand : IRequest<PayrollRecordDto>
    {
        public GeneratePayslipDto Payload { get; set; } = new();
    }

    public class GeneratePayslipCommandHandler : IRequestHandler<GeneratePayslipCommand, PayrollRecordDto>
    {
        private readonly IPayrollRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GeneratePayslipCommandHandler(IPayrollRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PayrollRecordDto> Handle(GeneratePayslipCommand request, CancellationToken cancellationToken)
        {
            var p = request.Payload;
            var record = new PayrollRecord
            {
                Id = Guid.NewGuid().ToString(),
                UserId = p.UserId,
                PayPeriod = p.PayPeriod,
                PayDate = p.PayDate,
                CountryCode = p.CountryCode,
                GrossPay = p.GrossPay,
                TotalDeductions = p.TotalDeductions,
                NetPay = p.NetPay,
                EarningsJson = p.EarningsJson,
                DeductionsJson = p.DeductionsJson,
                EmployerContributionsJson = p.EmployerContributionsJson,
                Status = "Approved",
                CreatedByUserId = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                    ?? _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == "sub")?.Value
                    ?? "",
                CreatedByUserName = "Admin",
                CreatedOn = DateTime.UtcNow
            };

            await _repository.CreatePayslipAsync(record);

            return new PayrollRecordDto
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
            };
        }
    }
}
