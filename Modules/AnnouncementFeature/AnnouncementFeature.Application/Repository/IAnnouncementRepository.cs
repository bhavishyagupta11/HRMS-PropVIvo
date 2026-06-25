using AnnouncementFeature.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnnouncementFeature.Application.Repository
{
    public interface IAnnouncementRepository
    {
        Task<IEnumerable<AnnouncementRecord>> GetActiveAnnouncementsAsync();
        Task PublishAnnouncementAsync(AnnouncementRecord record);
    }
}
