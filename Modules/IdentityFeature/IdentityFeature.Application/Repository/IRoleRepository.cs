using HRMS.Core.Postgres.Repositories;
using IdentityFeature.Domain;
using System.Threading.Tasks;

namespace IdentityFeature.Application.Repository
{
    public interface IRoleRepository : IPostgresRepository<Role>
    {
        Task<Role?> GetByNameAsync(string name);
    }
}
