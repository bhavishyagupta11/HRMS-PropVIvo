using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LeaveFeature.GraphQL.Mutations;
using LeaveFeature.GraphQL.Queries;

namespace LeaveFeature.GraphQL
{
    public static class LeaveGraphQLRegistration
    {
        public static IRequestExecutorBuilder AddLeaveGraphQL(this IRequestExecutorBuilder builder)
        {
            builder
                .AddTypeExtension<LeaveQueries>()
                .AddTypeExtension<LeaveMutations>();
            return builder;
        }
    }
}
