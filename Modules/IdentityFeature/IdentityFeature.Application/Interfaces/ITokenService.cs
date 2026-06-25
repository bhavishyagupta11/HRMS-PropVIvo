using IdentityFeature.Domain;
namespace IdentityFeature.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
