using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ContributionsFeature.GraphQL
{
    public static class ConfigureServiceExtension
    {
        public static IRequestExecutorBuilder AddContributionGraphQL(this IRequestExecutorBuilder builder)
        {
            builder.AddTypeExtension<ContributionQuery>()
                   .AddTypeExtension<ContributionMutation>();
            return builder;
        }
    }
}
