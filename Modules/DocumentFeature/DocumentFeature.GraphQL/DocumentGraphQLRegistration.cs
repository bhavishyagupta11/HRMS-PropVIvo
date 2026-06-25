using DocumentFeature.GraphQL.Mutations;
using DocumentFeature.GraphQL.Queries;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentFeature.GraphQL
{
    public static class DocumentGraphQLRegistration
    {
        public static IRequestExecutorBuilder AddDocumentGraphQL(this IRequestExecutorBuilder builder)
        {
            return builder
                .AddTypeExtension<DocumentQueries>()
                .AddTypeExtension<DocumentMutations>();
        }
    }
}
