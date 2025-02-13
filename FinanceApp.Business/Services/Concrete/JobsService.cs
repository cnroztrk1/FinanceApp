using Data.UnitOfWork;
using FinanceApp.Data;
using FinanceApp.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceApp.Business.Services
{
    public class JobService : IJobsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public JobService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Jobs>> GetAllJobsAsync()
        {
            return await _unitOfWork.Jobs.GetAllAsync();
        }

        public async Task<Jobs> GetJobByIdAsync(int id)
        {
            return await _unitOfWork.Jobs.GetByIdAsync(id);
        }

        public async Task CreateJobAsync(Jobs job)
        {
            await _unitOfWork.Jobs.AddAsync(job);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateJobAsync(Jobs job)
        {
            _unitOfWork.Jobs.Update(job);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteJobAsync(int id)
        {
            var job = await _unitOfWork.Jobs.GetByIdAsync(id);
            if (job != null)
            {
                _unitOfWork.Jobs.Remove(job);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
