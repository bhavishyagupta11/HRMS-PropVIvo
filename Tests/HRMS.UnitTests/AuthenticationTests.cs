using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityFeature.Application.Commands;
using IdentityFeature.Application.DTO;
using IdentityFeature.Application.Interfaces;
using IdentityFeature.Application.Repository;
using IdentityFeature.Domain;
using Moq;
using Xunit;

namespace HRMS.UnitTests
{
    public class AuthenticationTests
    {
        [Fact]
        public async Task LoginCommandHandler_WithValidCredentials_ReturnsExpectedTokenResponse()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var tokenServiceMock = new Mock<ITokenService>();

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = "john.doe@propvivo.com",
                PasswordHash = "hashed_password",
                FirstName = "John",
                LastName = "Doe",
                RoleId = "ROLE_EMPLOYEE"
            };

            userRepositoryMock.Setup(repo => repo.GetByEmailAsync("john.doe@propvivo.com"))
                .ReturnsAsync(user);

            passwordHasherMock.Setup(hasher => hasher.VerifyPassword("P@ssword123", "hashed_password"))
                .Returns(true);

            tokenServiceMock.Setup(service => service.GenerateToken(user))
                .Returns("mock_jwt_token_string");

            var handler = new LoginCommandHandler(userRepositoryMock.Object, passwordHasherMock.Object, tokenServiceMock.Object);
            var command = new LoginCommand
            {
                Request = new LoginRequest
                {
                    Email = "john.doe@propvivo.com",
                    Password = "P@ssword123"
                }
            };

            // Act
            var response = await handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Token.Should().Be("mock_jwt_token_string");
            response.Email.Should().Be("john.doe@propvivo.com");
            response.FirstName.Should().Be("John");
            response.LastName.Should().Be("Doe");
        }

        [Fact]
        public async Task LoginCommandHandler_WithInvalidCredentials_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var tokenServiceMock = new Mock<ITokenService>();

            userRepositoryMock.Setup(repo => repo.GetByEmailAsync("unknown@propvivo.com"))
                .ReturnsAsync((User?)null);

            var handler = new LoginCommandHandler(userRepositoryMock.Object, passwordHasherMock.Object, tokenServiceMock.Object);
            var command = new LoginCommand
            {
                Request = new LoginRequest
                {
                    Email = "unknown@propvivo.com",
                    Password = "WrongPassword"
                }
            };

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
