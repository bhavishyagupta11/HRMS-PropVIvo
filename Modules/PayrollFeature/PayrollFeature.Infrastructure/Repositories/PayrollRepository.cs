using HRMS.Core.Postgres.Data;
using Microsoft.EntityFrameworkCore;
using PayrollFeature.Application.Repository;
using PayrollFeature.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollFeature.Infrastructure.Repositories
{
    public class PayrollRepository : IPayrollRepository
    {
        private readonly PostgresDbContext _dbContext;

        public PayrollRepository(PostgresDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<PayrollRecord>> GetPayslipsAsync(string userId)
        {
            return await _dbContext.Set<PayrollRecord>()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.PayDate)
                .ToListAsync();
        }

        public async Task<PayrollRecord?> GetPayslipByIdAsync(string id)
        {
            return await _dbContext.Set<PayrollRecord>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task CreatePayslipAsync(PayrollRecord record)
        {
            await _dbContext.Set<PayrollRecord>().AddAsync(record);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdatePayslipAsync(PayrollRecord record)
        {
            _dbContext.Set<PayrollRecord>().Update(record);
            await _dbContext.SaveChangesAsync();
        }
    }
}
