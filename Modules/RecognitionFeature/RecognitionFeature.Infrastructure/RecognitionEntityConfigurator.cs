using HRMS.Core.Postgres.Interfaces;
using Microsoft.EntityFrameworkCore;
using RecognitionFeature.Domain;

namespace RecognitionFeature.Infrastructure
{
    public class RecognitionEntityConfigurator : IPostgresEntityConfigurator
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RecognitionRecord>(builder =>
            {
                builder.ToTable("Recognitions", "recognition");
                builder.HasKey(x => x.Id);
                builder.Property(x => x.GiverName).IsRequired();
                builder.Property(x => x.ReceiverName).IsRequired();
                builder.Property(x => x.Category).IsRequired();
            });
        }
    }
}
