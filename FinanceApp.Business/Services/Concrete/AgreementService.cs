using Data.UnitOfWork;
using FinanceApp.Data;
using FinanceApp.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceApp.Business.Services
{
    public class AgreementService : IAgreementService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AgreementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Agreement>> GetAllAgreementsAsync()
        {
            return await _unitOfWork.Agreements.GetAllAsync();
        }

        public async Task<Agreement> GetAgreementByIdAsync(int id)
        {
            return await _unitOfWork.Agreements.GetByIdAsync(id);
        }

        public async Task CreateAgreementAsync(Agreement agreement)
        {
            await _unitOfWork.Agreements.AddAsync(agreement);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAgreementAsync(Agreement agreement)
        {
            _unitOfWork.Agreements.Update(agreement);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAgreementAsync(int id)
        {
            var agreement = await _unitOfWork.Agreements.GetByIdAsync(id);
            if (agreement != null)
            {
                _unitOfWork.Agreements.Remove(agreement);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
