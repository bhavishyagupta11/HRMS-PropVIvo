using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnboardingFeature.GraphQL.Mutations;
using OnboardingFeature.GraphQL.Queries;

namespace OnboardingFeature.GraphQL
{
    public static class OnboardingGraphQLRegistration
    {
        public static IRequestExecutorBuilder AddOnboardingGraphQL(this IRequestExecutorBuilder builder)
        {
            builder
                .AddTypeExtension<OnboardingQueries>()
                .AddTypeExtension<OnboardingMutations>();
            return builder;
        }
    }
}
