using Data.UnitOfWork;
using FinanceApp.Common;
using FinanceApp.Data.Entities;

namespace FinanceApp.Business.Services
{
    public class JobService : IJobsService // İş konuları için Create Read Update Delete (CRUD) işlemleri
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;
        private readonly int _tenantId;
        private readonly string _cacheKey = "jobs_{0}"; // Tenant bazlı cache key

        public JobService(IUnitOfWork unitOfWork, ITenantProvider tenantProvider, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _tenantId = tenantProvider.TenantId;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<Jobs>> GetAllJobsAsync()
        {
            string cacheKey = string.Format(_cacheKey, _tenantId);
            return await _cacheService.GetOrCreateAsync(cacheKey, async () =>
            {
                var jobs = await _unitOfWork.Jobs.GetAllAsync();
                foreach (var job in jobs)
                {
                    job.Agreement = await _unitOfWork.Agreements.GetByIdAsync(job.AgreementId ?? 0);
                    job.BusinessPartner = await _unitOfWork.Partners.GetByIdAsync(job.BusinessPartnerId ?? 0);
                }
                return jobs.Where(j => j.TenantId == _tenantId);
            }, TimeSpan.FromMinutes(30)); // 30 dakika cache'de tut
        }

        public async Task<Jobs> GetJobByIdAsync(int id)
        {
            var job = await _unitOfWork.Jobs.GetByIdAsync(id);
            return job?.TenantId == _tenantId ? job : null;
        }

        public async Task CreateJobAsync(Jobs job)
        {
            job.TenantId = job.TenantId == 0 ? _tenantId : job.TenantId;
            await _unitOfWork.Jobs.AddAsync(job);
            await _unitOfWork.CompleteAsync();
            _cacheService.Remove(string.Format(_cacheKey, _tenantId)); // Cache temizleme
        }

        public async Task UpdateJobAsync(Jobs job)
        {
            if (job.TenantId == _tenantId)
            {
                _unitOfWork.Jobs.Update(job);
                await _unitOfWork.CompleteAsync();
                _cacheService.Remove(string.Format(_cacheKey, _tenantId)); // Güncelleme sonrası cache temizleme
            }
        }

        public async Task DeleteJobAsync(int id)
        {
            var job = await _unitOfWork.Jobs.GetByIdAsync(id);
            if (job != null && job.TenantId == _tenantId)
            {
                _unitOfWork.Jobs.Remove(job);
                await _unitOfWork.CompleteAsync();
                _cacheService.Remove(string.Format(_cacheKey, _tenantId)); // Silme sonrası cache temizleme
            }
        }
    }
}
