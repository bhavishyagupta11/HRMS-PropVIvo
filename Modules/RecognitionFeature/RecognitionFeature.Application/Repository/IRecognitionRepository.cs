using RecognitionFeature.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecognitionFeature.Application.Repository
{
    public interface IRecognitionRepository
    {
        Task<IEnumerable<RecognitionRecord>> GetRecentRecognitionsAsync();
        Task SendRecognitionAsync(RecognitionRecord record);
    }
}
