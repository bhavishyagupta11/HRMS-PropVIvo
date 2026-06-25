using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AttendanceFeature.GraphQL.Mutations;
using AttendanceFeature.GraphQL.Queries;

namespace AttendanceFeature.GraphQL
{
    public static class AttendanceGraphQLRegistration
    {
        public static IRequestExecutorBuilder AddAttendanceGraphQL(this IRequestExecutorBuilder builder)
        {
            builder.AddTypeExtension<AttendanceQueries>();
            builder.AddTypeExtension<AttendanceMutations>();
            return builder;
        }
    }
}
