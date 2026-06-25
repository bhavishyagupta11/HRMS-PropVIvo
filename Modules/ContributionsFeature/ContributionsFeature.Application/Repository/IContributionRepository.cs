using ContributionsFeature.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContributionsFeature.Application.Repository
{
    public interface IContributionRepository
    {
        Task<IEnumerable<ValueContribution>> GetContributionsAsync();
        Task<IEnumerable<ContributionItem>> GetAvailableItemsAsync();
        Task<IEnumerable<ContributionLeaderboard>> GetLeaderboardAsync();
        Task<ContributionItem> ClaimContributionItemAsync(string itemId, string employeeName);
        Task<ValueContribution> ApproveContributionAsync(string contributionId, int finalPoints, string comments, string approverName);
        Task<ValueContribution> SubmitContributionAsync(ValueContribution contribution);
    }
}
