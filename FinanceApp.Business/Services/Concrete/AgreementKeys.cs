using Data.UnitOfWork;
using FinanceApp.Common;
using FinanceApp.Data.Entities;
using System.Linq;

namespace FinanceApp.Business.Services
{
    public class AgreementKeysService : IAgreementKeysService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly int _tenantId;

        public AgreementKeysService(IUnitOfWork unitOfWork, ITenantProvider tenantProvider)
        {
            _unitOfWork = unitOfWork;
            _tenantId = tenantProvider.TenantId;
        }

        public async Task<IEnumerable<AgreementKeys>> GetAllKeywordsAsync()
        {
            return (await _unitOfWork.AgreementKeys.GetAllAsync()).Where(k => k.TenantId == _tenantId);
        }

        public async Task<AgreementKeys> GetKeywordByIdAsync(int id)
        {
            var keyword = await _unitOfWork.AgreementKeys.GetByIdAsync(id);
            return keyword?.TenantId == _tenantId ? keyword : null;
        }

        public async Task CreateKeywordAsync(AgreementKeys keyword)
        {
            keyword.TenantId = _tenantId;
            await _unitOfWork.AgreementKeys.AddAsync(keyword);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateKeywordAsync(AgreementKeys keyword)
        {
            if (keyword.TenantId == _tenantId)
            {
                _unitOfWork.AgreementKeys.Update(keyword);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task DeleteKeywordAsync(int id)
        {
            var keyword = await _unitOfWork.AgreementKeys.GetByIdAsync(id);
            if (keyword != null && keyword.TenantId == _tenantId)
            {
                _unitOfWork.AgreementKeys.Remove(keyword);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
