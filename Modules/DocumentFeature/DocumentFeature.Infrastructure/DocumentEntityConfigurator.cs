using DocumentFeature.Domain;
using HRMS.Core.Postgres.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DocumentFeature.Infrastructure
{
    public class DocumentEntityConfigurator : IPostgresEntityConfigurator
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DocumentRecord>(builder =>
            {
                builder.ToTable("DocumentRecords", "documents");
                builder.HasKey(x => x.Id);
                builder.Property(x => x.UserId).IsRequired();
                builder.Property(x => x.DocumentType).IsRequired();
                builder.Property(x => x.FileName).IsRequired();
                builder.Property(x => x.BlobUrl).IsRequired();
            });
        }
    }
}
