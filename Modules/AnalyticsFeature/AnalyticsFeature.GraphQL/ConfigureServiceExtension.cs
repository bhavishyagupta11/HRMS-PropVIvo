using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AnalyticsFeature.GraphQL
{
    public static class ConfigureServiceExtension
    {
        public static IRequestExecutorBuilder AddAnalyticsGraphQL(this IRequestExecutorBuilder builder)
        {
            builder.AddTypeExtension<AnalyticsQuery>();
            return builder;
        }
    }
}
