using HRMS.Core.Postgres.Interfaces;
using Microsoft.EntityFrameworkCore;
using TrainingFeature.Domain;

namespace TrainingFeature.Infrastructure
{
    public class TrainingEntityConfigurator : IPostgresEntityConfigurator
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TrainingCourseRecord>(builder =>
            {
                builder.ToTable("TrainingCourses", "training");
                builder.HasKey(x => x.Id);
                builder.Property(x => x.Title).IsRequired();
                builder.Property(x => x.Description).IsRequired();
                builder.Property(x => x.TrainerName).IsRequired();
            });

            modelBuilder.Entity<CourseEnrollmentRecord>(builder =>
            {
                builder.ToTable("CourseEnrollments", "training");
                builder.HasKey(x => x.Id);
                builder.Property(x => x.CourseId).IsRequired();
                builder.Property(x => x.UserId).IsRequired();
                builder.Property(x => x.Status).IsRequired();
            });
        }
    }
}
