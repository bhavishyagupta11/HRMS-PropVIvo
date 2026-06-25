using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RecognitionFeature.GraphQL
{
    public static class ConfigureServiceExtension
    {
        public static IRequestExecutorBuilder AddRecognitionGraphQL(this IRequestExecutorBuilder builder)
        {
            builder.AddTypeExtension<RecognitionQuery>()
                   .AddTypeExtension<RecognitionMutation>();
            return builder;
        }
    }
}
