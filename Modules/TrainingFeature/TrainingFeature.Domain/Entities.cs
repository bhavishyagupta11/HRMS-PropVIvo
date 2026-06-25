using HRMS.Core.Postgres.Common;
using System;

namespace TrainingFeature.Domain
{
    public class TrainingCourseRecord : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TrainerName { get; set; } = string.Empty;
        public int Credits { get; set; }
        public DateTime ScheduleDate { get; set; }
    }

    public class CourseEnrollmentRecord : BaseEntity
    {
        public string CourseId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
        public DateTime? CompletionDate { get; set; }
    }
}
