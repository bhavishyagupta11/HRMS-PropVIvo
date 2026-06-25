using IdentityFeature.Application.Interfaces;
using IdentityFeature.Domain;
using Microsoft.AspNetCore.Identity;

namespace IdentityFeature.Infrastructure.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly PasswordHasher<User> _hasher = new PasswordHasher<User>();

        public string HashPassword(string password)
        {
            // We use a dummy user object since the hasher needs it for interface compliance but typically uses it for salt internally if configured
            var dummyUser = new User();
            return _hasher.HashPassword(dummyUser, password);
        }

        public bool VerifyPassword(string password, string hash)
        {
            var dummyUser = new User();
            var result = _hasher.VerifyHashedPassword(dummyUser, hash, password);
            return result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}
