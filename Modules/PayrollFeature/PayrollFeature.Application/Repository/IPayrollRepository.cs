using PayrollFeature.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PayrollFeature.Application.Repository
{
    public interface IPayrollRepository
    {
        Task<IEnumerable<PayrollRecord>> GetPayslipsAsync(string userId);
        Task<PayrollRecord?> GetPayslipByIdAsync(string id);
        Task CreatePayslipAsync(PayrollRecord record);
        Task UpdatePayslipAsync(PayrollRecord record);
    }
}
