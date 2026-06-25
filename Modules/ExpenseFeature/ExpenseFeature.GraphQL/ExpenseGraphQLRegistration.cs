using ExpenseFeature.GraphQL.Mutations;
using ExpenseFeature.GraphQL.Queries;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseFeature.GraphQL
{
    public static class ExpenseGraphQLRegistration
    {
        public static IRequestExecutorBuilder AddExpenseGraphQL(this IRequestExecutorBuilder builder)
        {
            return builder
                .AddTypeExtension<ExpenseQueries>()
                .AddTypeExtension<ExpenseMutations>();
        }
    }
}
