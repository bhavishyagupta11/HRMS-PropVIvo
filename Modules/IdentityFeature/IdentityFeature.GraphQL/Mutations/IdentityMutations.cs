using HotChocolate.Types;
using IdentityFeature.Application.Commands;
using IdentityFeature.Application.DTO;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using IdentityFeature.Domain;

namespace IdentityFeature.GraphQL.Mutations
{
    [ExtendObjectType("Mutation")]
    public class IdentityMutations
    {
        public async Task<LoginResponse> LoginAsync(
            LoginRequest request,
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            return await mediator.Send(new LoginCommand { Request = request }, cancellationToken);
        }

        public async Task<bool> RegisterAsync(
            string email, string password, string firstName, string lastName,
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            return await mediator.Send(new RegisterCommand { Email = email, Password = password, FirstName = firstName, LastName = lastName }, cancellationToken);
        }
    }
}
