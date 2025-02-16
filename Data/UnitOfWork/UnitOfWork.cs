using Data.Repos;
using FinanceApp.Data;
using FinanceApp.Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FinanceAppContext _context;
        private readonly int _tenantId;

        public IRepository<Agreement> Agreements { get; private set; }
        public IRepository<AgreementKeys> AgreementKeys { get; private set; }
        public IRepository<Jobs> Jobs { get; private set; }
        public IRepository<Partners> Partners { get; private set; }
        public IRepository<RiskAnalysis> RiskAnalysis { get; private set; }

        public IRepository<Company> Companies { get; private set; }

        public UnitOfWork(FinanceAppContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;

            // TenantId'yi HTTP Header'dan veya Cookie'den al
            var tenantIdString = httpContextAccessor.HttpContext?.Request.Headers["TenantId"].ToString();
            if (!int.TryParse(tenantIdString, out _tenantId))
            {
                _tenantId = 1; // Varsayılan TenantId
            }

            Agreements = new GenericRepository<Agreement>(context, _tenantId);
            AgreementKeys = new GenericRepository<AgreementKeys>(context, _tenantId);
            Jobs = new GenericRepository<Jobs>(context, _tenantId);
            Partners = new GenericRepository<Partners>(context, _tenantId);
            RiskAnalysis = new GenericRepository<RiskAnalysis>(context, _tenantId);
            Companies = new GenericRepository<Company>(context, _tenantId);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
