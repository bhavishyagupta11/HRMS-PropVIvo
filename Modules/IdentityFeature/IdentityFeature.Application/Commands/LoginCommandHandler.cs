using MediatR;
using IdentityFeature.Application.DTO;
using IdentityFeature.Application.Interfaces;
using IdentityFeature.Application.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityFeature.Application.Commands
{
    public class LoginCommand : IRequest<LoginResponse>
    {
        public LoginRequest Request { get; set; } = null!;
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Request.Email);
            if (user == null || !_passwordHasher.VerifyPassword(request.Request.Password, user.PasswordHash))
            {
                throw new System.UnauthorizedAccessException("Invalid email or password.");
            }

            var token = _tokenService.GenerateToken(user);
            return new LoginResponse
            {
                Token = token,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }
    }
}
