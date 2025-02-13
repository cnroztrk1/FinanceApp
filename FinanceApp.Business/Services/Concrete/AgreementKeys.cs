using Data.UnitOfWork;
using FinanceApp.Data;
using FinanceApp.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceApp.Business.Services
{
    public class AgreementKeysService : IAgreementKeysService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AgreementKeysService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AgreementKeys>> GetAllKeywordsAsync()
        {
            return await _unitOfWork.AgreementKeys.GetAllAsync();
        }

        public async Task<AgreementKeys> GetKeywordByIdAsync(int id)
        {
            return await _unitOfWork.AgreementKeys.GetByIdAsync(id);
        }

        public async Task CreateKeywordAsync(AgreementKeys keyword)
        {
            await _unitOfWork.AgreementKeys.AddAsync(keyword);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateKeywordAsync(AgreementKeys keyword)
        {
            _unitOfWork.AgreementKeys.Update(keyword);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteKeywordAsync(int id)
        {
            var keyword = await _unitOfWork.AgreementKeys.GetByIdAsync(id);
            if (keyword != null)
            {
                _unitOfWork.AgreementKeys.Remove(keyword);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
