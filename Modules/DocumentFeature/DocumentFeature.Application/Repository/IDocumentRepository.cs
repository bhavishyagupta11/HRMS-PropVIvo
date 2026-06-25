using DocumentFeature.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentFeature.Application.Repository
{
    public interface IDocumentRepository
    {
        Task<DocumentRecord?> GetByIdAsync(string id, CancellationToken cancellationToken);
        Task<IEnumerable<DocumentRecord>> GetByUserIdAsync(string userId, CancellationToken cancellationToken);
        Task<IEnumerable<DocumentRecord>> GetPendingDocumentsAsync(CancellationToken cancellationToken);
        Task<DocumentRecord> AddAsync(DocumentRecord record, CancellationToken cancellationToken);
        Task<DocumentRecord> UpdateAsync(DocumentRecord record, CancellationToken cancellationToken);
    }
}
