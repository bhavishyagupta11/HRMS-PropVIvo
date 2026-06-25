using HotChocolate.Execution.Configuration;

namespace HRMS.API.RegisterDependencies
{
    public static class GraphQLModuleRegistration
    {
        public static IRequestExecutorBuilder AddGraphQLModules(this IRequestExecutorBuilder builder)
        {
            // All module GraphQL registrations are handled in GraphQLExtensions.ConfigureGraphQL.
            // This extension point is available for future non-module GraphQL additions.
            return builder;
        }
    }
}
