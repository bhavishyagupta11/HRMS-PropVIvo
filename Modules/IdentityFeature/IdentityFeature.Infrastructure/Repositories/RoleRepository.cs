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
    public class RoleRepository : PostgresDbRepository<Role>, IRoleRepository
    {
        public RoleRepository(
            PostgresDbContext context,
            ILogger<RoleRepository> logger,
            ITelemetryService telemetryService,
            IHttpContextAccessor httpContextAccessor)
            : base(context, logger, telemetryService, httpContextAccessor)
        { }

        public override string TableName { get; } = "Roles";

        public override string GenerateId(Role entity) => Guid.NewGuid().ToString();

        public async Task<Role?> GetByNameAsync(string name)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Name == name);
        }
    }
}
