using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Data;
using RestaurantManagement.IRepositories;
using RestaurantManagement.Models;

namespace RestaurantManagement.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseModel
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

        public void Update(T obj,Guid createdById, CancellationToken cancellationToken = default)
        {
            obj.IsUpdated = true;
            obj.UpdatedAt = DateTime.UtcNow;
            obj.UpdatedById = createdById;
            _dbSet.Update(obj);
        }

        public void Delete(T obj,Guid createdById, CancellationToken cancellationToken = default)
        {
            obj.IsDeleted = true;
            obj.DeletedAt = DateTime.UtcNow;
            obj.DeletedById = createdById;
            _dbSet.Update(obj);
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await GetByIdAsync(id) != null;
        }

    }
}