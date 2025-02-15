using Data.UnitOfWork;
using FinanceApp.Data.Entities;
using FinanceApp.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceApp.Business.Services
{
    public class JobService : IJobsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly int _tenantId;

        public JobService(IUnitOfWork unitOfWork, ITenantProvider tenantProvider)
        {
            _unitOfWork = unitOfWork;
            _tenantId = tenantProvider.TenantId;
        }

        public async Task<IEnumerable<Jobs>> GetAllJobsAsync()
        {
            return (await _unitOfWork.Jobs.GetAllAsync()).Where(j => j.TenantId == _tenantId);
        }

        public async Task<Jobs> GetJobByIdAsync(int id)
        {
            var job = await _unitOfWork.Jobs.GetByIdAsync(id);
            return job?.TenantId == _tenantId ? job : null;
        }

        public async Task CreateJobAsync(Jobs job)
        {
            job.TenantId = _tenantId;
            await _unitOfWork.Jobs.AddAsync(job);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateJobAsync(Jobs job)
        {
            if (job.TenantId == _tenantId)
            {
                _unitOfWork.Jobs.Update(job);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task DeleteJobAsync(int id)
        {
            var job = await _unitOfWork.Jobs.GetByIdAsync(id);
            if (job != null && job.TenantId == _tenantId)
            {
                _unitOfWork.Jobs.Remove(job);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
