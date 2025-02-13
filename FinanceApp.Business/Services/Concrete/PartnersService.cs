using Data.UnitOfWork;
using FinanceApp.Data;
using FinanceApp.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceApp.Business.Services
{
    public class BusinessPartnerService : IPartnersService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BusinessPartnerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Partners>> GetAllPartnersAsync()
        {
            return await _unitOfWork.Partners.GetAllAsync();
        }

        public async Task<Partners> GetPartnerByIdAsync(int id)
        {
            return await _unitOfWork.Partners.GetByIdAsync(id);
        }

        public async Task CreatePartnerAsync(Partners partner)
        {
            await _unitOfWork.Partners.AddAsync(partner);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdatePartnerAsync(Partners partner)
        {
            _unitOfWork.Partners.Update(partner);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeletePartnerAsync(int id)
        {
            var partner = await _unitOfWork.Partners.GetByIdAsync(id);
            if (partner != null)
            {
                _unitOfWork.Partners.Remove(partner);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
