using AnalyticsFeature.Application.DTO;
using AnalyticsFeature.Application.Queries;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using System.Threading.Tasks;

namespace AnalyticsFeature.GraphQL
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class AnalyticsQuery
    {
        [Authorize]
        public async Task<AnalyticsDashboardDto> GetAnalyticsDashboard([Service] IMediator mediator)
        {
            return await mediator.Send(new GetAnalyticsDashboardQuery());
        }
    }
}
