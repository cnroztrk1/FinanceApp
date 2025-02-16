using Data.Repos;
using FinanceApp.Data;
using FinanceApp.Data.Entities;
using Microsoft.AspNetCore.Http;
using System;

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

            var tenantId = httpContextAccessor.HttpContext.Session.GetInt32("TenantId");

            if (tenantId == null || tenantId == 0)
            {
                _tenantId = 1; // Varsayılan TenantId
            }
            else
            {
                _tenantId = tenantId.Value;
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
