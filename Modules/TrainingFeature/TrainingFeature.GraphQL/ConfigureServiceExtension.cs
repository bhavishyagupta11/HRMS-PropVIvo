using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TrainingFeature.GraphQL
{
    public static class ConfigureServiceExtension
    {
        public static IRequestExecutorBuilder AddTrainingGraphQL(this IRequestExecutorBuilder builder)
        {
            builder.AddTypeExtension<TrainingQuery>()
                   .AddTypeExtension<TrainingMutation>();
            return builder;
        }
    }
}
