using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RecruitmentFeature.GraphQL
{
    public static class ConfigureServiceExtension
    {
        public static IRequestExecutorBuilder AddRecruitmentGraphQL(this IRequestExecutorBuilder builder)
        {
            builder.AddTypeExtension<RecruitmentQuery>()
                   .AddTypeExtension<RecruitmentMutation>();
            return builder;
        }
    }
}
