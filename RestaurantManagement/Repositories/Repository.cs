using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Data;
using RestaurantManagement.IRepositories;

namespace RestaurantManagement.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbSet<T> _dbSet;
        private readonly AppDbContext _context;
        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T obj, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(obj);
        }

        public void Update(T obj, CancellationToken cancellationToken = default)
        {
            _dbSet.Update(obj);
        }

        public void Delete(T obj, CancellationToken cancellationToken = default)
        {
            _dbSet.Remove(obj);
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await GetByIdAsync(id) != null;
        }

    }
}