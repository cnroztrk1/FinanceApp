using Data.UnitOfWork;
using FinanceApp.Data.Entities;
using FinanceApp.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceApp.Business.Services
{
    public class AgreementService : IAgreementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly int _tenantId;

        public AgreementService(IUnitOfWork unitOfWork, ITenantProvider tenantProvider)
        {
            _unitOfWork = unitOfWork;
            _tenantId = tenantProvider.TenantId;
        }

        public async Task<IEnumerable<Agreement>> GetAllAgreementsAsync()
        {
            return (await _unitOfWork.Agreements.GetAllAsync()).Where(a => a.TenantId == _tenantId);
        }

        public async Task<Agreement> GetAgreementByIdAsync(int id)
        {
            var agreement = await _unitOfWork.Agreements.GetByIdAsync(id);
            return agreement?.TenantId == _tenantId ? agreement : null;
        }

        public async Task CreateAgreementAsync(Agreement agreement)
        {
            agreement.TenantId = _tenantId;
            await _unitOfWork.Agreements.AddAsync(agreement);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAgreementAsync(Agreement agreement)
        {
            if (agreement.TenantId == _tenantId)
            {
                _unitOfWork.Agreements.Update(agreement);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task DeleteAgreementAsync(int id)
        {
            var agreement = await _unitOfWork.Agreements.GetByIdAsync(id);
            if (agreement != null && agreement.TenantId == _tenantId)
            {
                _unitOfWork.Agreements.Remove(agreement);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
