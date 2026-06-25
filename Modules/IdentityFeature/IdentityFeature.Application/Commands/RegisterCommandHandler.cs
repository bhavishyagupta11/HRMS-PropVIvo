using MediatR;
using IdentityFeature.Application.Interfaces;
using IdentityFeature.Application.Repository;
using IdentityFeature.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityFeature.Application.Commands
{
    public class RegisterCommand : IRequest<bool>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user != null) throw new System.Exception("User already exists");

            var newUser = new User
            {
                Id = System.Guid.NewGuid().ToString(),
                Email = request.Email,
                PasswordHash = _passwordHasher.HashPassword(request.Password),
                FirstName = request.FirstName,
                LastName = request.LastName,
                RoleId = "Employee",
                CreatedOn = System.DateTime.UtcNow,
                UserContext = new HRMS.Shared.Domain.Entity.UserBase
                {
                    CreatedByUserId = System.Guid.Empty.ToString()
                }
            };

            await _userRepository.AddItemAsync(newUser);
            return true;
        }
    }
}
