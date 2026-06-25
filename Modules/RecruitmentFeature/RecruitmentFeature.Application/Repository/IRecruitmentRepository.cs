using RecruitmentFeature.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecruitmentFeature.Application.Repository
{
    public interface IRecruitmentRepository
    {
        Task<IEnumerable<JobRequisitionRecord>> GetOpenJobsAsync();
        Task<IEnumerable<CandidateApplicationRecord>> GetApplicationsAsync(string email);
        Task ApplyAsync(CandidateApplicationRecord record);
        Task<CandidateApplicationRecord?> GetApplicationByIdAsync(string applicationId);
        Task UpdateApplicationAsync(CandidateApplicationRecord record);
    }
}
