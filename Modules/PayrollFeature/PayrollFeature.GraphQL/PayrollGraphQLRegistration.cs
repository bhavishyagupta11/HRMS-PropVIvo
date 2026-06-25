using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PayrollFeature.GraphQL.Mutations;
using PayrollFeature.GraphQL.Queries;

namespace PayrollFeature.GraphQL
{
    public static class PayrollGraphQLRegistration
    {
        public static IRequestExecutorBuilder AddPayrollGraphQL(this IRequestExecutorBuilder builder)
        {
            builder
                .AddTypeExtension<PayrollQueries>()
                .AddTypeExtension<PayrollMutations>();
            return builder;
        }
    }
}
