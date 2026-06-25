using Microsoft.Extensions.DependencyInjection;
using HotChocolate.Execution.Configuration;

namespace PerformanceFeature.GraphQL
{
    public static class ConfigureServiceExtension
    {
        public static IRequestExecutorBuilder AddPerformanceGraphQL(this IRequestExecutorBuilder builder)
        {
            builder.AddTypeExtension<PerformanceQuery>()
                   .AddTypeExtension<PerformanceMutation>();
            return builder;
        }
    }
}
