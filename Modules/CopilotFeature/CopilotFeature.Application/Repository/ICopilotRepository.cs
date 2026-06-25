using CopilotFeature.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CopilotFeature.Application.Repository
{
    public interface ICopilotRepository
    {
        Task<IEnumerable<CopilotInteractionRecord>> GetInteractionHistoryAsync();
        Task SaveInteractionAsync(CopilotInteractionRecord record);
    }
}
