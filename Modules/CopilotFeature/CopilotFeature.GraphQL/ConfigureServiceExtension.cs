using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CopilotFeature.GraphQL
{
    public static class ConfigureServiceExtension
    {
        public static IRequestExecutorBuilder AddCopilotGraphQL(this IRequestExecutorBuilder builder)
        {
            builder.AddTypeExtension<CopilotQuery>()
                   .AddTypeExtension<CopilotMutation>();
            return builder;
        }
    }
}
