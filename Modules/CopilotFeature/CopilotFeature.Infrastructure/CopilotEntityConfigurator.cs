using CopilotFeature.Domain;
using HRMS.Core.Postgres.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CopilotFeature.Infrastructure
{
    public class CopilotEntityConfigurator : IPostgresEntityConfigurator
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CopilotInteractionRecord>(builder =>
            {
                builder.ToTable("CopilotInteractions", "copilot");
                builder.HasKey(x => x.Id);
                builder.Property(x => x.PromptText).IsRequired();
                builder.Property(x => x.ResponseText).IsRequired();
                builder.Property(x => x.Category).IsRequired();
            });
        }
    }
}
