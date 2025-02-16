using Data.UnitOfWork;
using FinanceApp.Common;
using FinanceApp.Data.Entities;
using System.Linq;

namespace FinanceApp.Business.Services
{
    public class JobService : IJobsService //İş konuları için Create Read Update Delete (CRUD) işlemleri
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
            var jobs = await _unitOfWork.Jobs.GetAllAsync();

            foreach (var job in jobs)
            {
                job.Agreement = await _unitOfWork.Agreements.GetByIdAsync(job.AgreementId ?? 0);
                job.BusinessPartner = await _unitOfWork.Partners.GetByIdAsync(job.BusinessPartnerId ?? 0);
            }

            return jobs.Where(j => j.TenantId == _tenantId);
        }


        public async Task<Jobs> GetJobByIdAsync(int id)
        {
            var job = await _unitOfWork.Jobs.GetByIdAsync(id);
            return job?.TenantId == _tenantId ? job : null;
        }

        public async Task CreateJobAsync(Jobs job)
        {
            job.TenantId = job.TenantId==0 ? _tenantId : job.TenantId;
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
