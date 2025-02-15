using FinanceApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repos
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected readonly FinanceAppContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly int _tenantId;

        public GenericRepository(FinanceAppContext context, int tenantId)
        {
            _context = context;
            _dbSet = context.Set<T>();
            _tenantId = tenantId;
        }

        public async Task<T> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.Where(e => EF.Property<int>(e, "TenantId") == _tenantId).ToListAsync();

        public async Task AddAsync(T entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
        }

        public void Update(T entity) => _dbSet.Update(entity);

        public void Remove(T entity) => _dbSet.Remove(entity);
    }
}
