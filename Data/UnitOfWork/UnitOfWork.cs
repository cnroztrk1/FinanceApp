using Data.Repos;
using FinanceApp.Data.Entities;
using FinanceApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FinanceAppContext _context;
        public IRepository<Agreement> Agreements { get; private set; }
        public IRepository<AgreementKeys> AgreementKeys { get; private set; }

        public IRepository<Jobs> Jobs { get; private set; }

        public IRepository<Partners> Partners { get; private set; }

        public IRepository<RiskAnalysis> RiskAnalysis { get; private set; }

        public UnitOfWork(FinanceAppContext context)
        {
            _context = context;
            Agreements = new GenericRepository<Agreement>(context);
            AgreementKeys = new GenericRepository<AgreementKeys>(context);
            Jobs = new GenericRepository<Jobs>(context);
            Partners = new GenericRepository<Partners>(context);
            RiskAnalysis = new GenericRepository<RiskAnalysis>(context);
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
