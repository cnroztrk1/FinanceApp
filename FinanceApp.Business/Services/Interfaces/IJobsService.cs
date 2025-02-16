using FinanceApp.Data.Entities;

namespace FinanceApp.Business.Services
{
    public interface IJobsService
    {
        Task<IEnumerable<Jobs>> GetAllJobsAsync();
        Task<Jobs> GetJobByIdAsync(int id);
        Task CreateJobAsync(Jobs job);
        Task UpdateJobAsync(Jobs job);
        Task DeleteJobAsync(int id);
    }
}
