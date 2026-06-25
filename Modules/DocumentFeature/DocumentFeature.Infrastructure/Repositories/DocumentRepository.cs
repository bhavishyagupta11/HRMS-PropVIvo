using DocumentFeature.Application.Repository;
using DocumentFeature.Domain;
using HRMS.Core.Postgres.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentFeature.Infrastructure.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly PostgresDbContext _context;

        public DocumentRepository(PostgresDbContext context)
        {
            _context = context;
        }

        public async Task<DocumentRecord?> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await _context.Set<DocumentRecord>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<DocumentRecord>> GetByUserIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await _context.Set<DocumentRecord>()
                .Where(x => x.UserId == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<DocumentRecord>> GetPendingDocumentsAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<DocumentRecord>()
                .Where(x => x.VerificationStatus == "Uploaded")
                .ToListAsync(cancellationToken);
        }

        public async Task<DocumentRecord> AddAsync(DocumentRecord record, CancellationToken cancellationToken)
        {
            await _context.Set<DocumentRecord>().AddAsync(record, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return record;
        }

        public async Task<DocumentRecord> UpdateAsync(DocumentRecord record, CancellationToken cancellationToken)
        {
            _context.Set<DocumentRecord>().Update(record);
            await _context.SaveChangesAsync(cancellationToken);
            return record;
        }
    }
}
