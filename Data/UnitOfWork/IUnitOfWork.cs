using Data.Repos;
using FinanceApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Agreement> Agreements { get; }

        IRepository<AgreementKeys> AgreementKeys { get; }

        IRepository<Jobs> Jobs { get; }
        IRepository<Partners> Partners { get; }
        IRepository<RiskAnalysis> RiskAnalysis { get; }

        IRepository<Company> Companies { get; }
        Task<int> CompleteAsync();
    }

}
