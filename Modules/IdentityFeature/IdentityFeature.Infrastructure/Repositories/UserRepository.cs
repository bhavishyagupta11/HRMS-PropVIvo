using HRMS.Core.Postgres.Data;
using HRMS.Core.Postgres.Repositories;
using HRMS.Core.Telemetry;
using IdentityFeature.Application.Repository;
using IdentityFeature.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;

namespace IdentityFeature.Infrastructure.Repositories
{
    public class UserRepository : PostgresDbRepository<User>, IUserRepository
    {
        public UserRepository(
            PostgresDbContext context,
            ILogger<UserRepository> logger,
            ITelemetryService telemetryService,
            IHttpContextAccessor httpContextAccessor)
            : base(context, logger, telemetryService, httpContextAccessor)
        { }

        public override string TableName { get; } = "Users";

        public override string GenerateId(User entity) => Guid.NewGuid().ToString();

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.AsNoTracking().Include(x => x.Role).FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
