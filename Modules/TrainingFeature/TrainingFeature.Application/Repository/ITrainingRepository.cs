using System.Collections.Generic;
using System.Threading.Tasks;
using TrainingFeature.Domain;

namespace TrainingFeature.Application.Repository
{
    public interface ITrainingRepository
    {
        Task<IEnumerable<TrainingCourseRecord>> GetAvailableCoursesAsync();
        Task<IEnumerable<CourseEnrollmentRecord>> GetMyEnrollmentsAsync(string userId);
        Task EnrollAsync(CourseEnrollmentRecord record);
        Task<CourseEnrollmentRecord?> GetEnrollmentAsync(string enrollmentId, string userId);
        Task UpdateEnrollmentAsync(CourseEnrollmentRecord record);
    }
}
