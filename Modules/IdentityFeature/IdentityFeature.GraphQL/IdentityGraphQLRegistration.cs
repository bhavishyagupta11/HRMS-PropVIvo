using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IdentityFeature.GraphQL.Mutations;
using IdentityFeature.GraphQL.Queries;

namespace IdentityFeature.GraphQL
{
    public static class IdentityGraphQLRegistration
    {
        public static IRequestExecutorBuilder AddIdentityGraphQL(this IRequestExecutorBuilder builder)
        {
            builder.AddTypeExtension<IdentityMutations>();
            builder.AddTypeExtension<IdentityQueries>();
            return builder;
        }
    }
}
