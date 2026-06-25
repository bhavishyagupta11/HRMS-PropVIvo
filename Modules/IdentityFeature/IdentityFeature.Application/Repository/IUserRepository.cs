using HRMS.Core.Postgres.Repositories;
using IdentityFeature.Domain;
using System.Threading.Tasks;

namespace IdentityFeature.Application.Repository
{
    public interface IUserRepository : IPostgresRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
    }
}
