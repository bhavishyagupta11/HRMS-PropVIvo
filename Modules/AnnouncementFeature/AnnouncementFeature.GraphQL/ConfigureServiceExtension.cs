using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AnnouncementFeature.GraphQL
{
    public static class ConfigureServiceExtension
    {
        public static IRequestExecutorBuilder AddAnnouncementGraphQL(this IRequestExecutorBuilder builder)
        {
            builder.AddTypeExtension<AnnouncementQuery>()
                   .AddTypeExtension<AnnouncementMutation>();
            return builder;
        }
    }
}
