using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Data;
using RestaurantManagement.Extensions;
using RestaurantManagement.IRepositories;
using RestaurantManagement.Models;

namespace RestaurantManagement.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseModel
    {
        protected readonly DbSet<T> _dbSet;
        //private readonly AppDbContext _context;
        private Guid? userId;
        public Repository(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            //_context = context;
            _dbSet = context.Set<T>();
            userId = httpContextAccessor.HttpContext?.User.GetUserId();
        }


        public IQueryable<T> Select(DeletedStatus status = DeletedStatus.NotDeleted)
        {
            if (status == DeletedStatus.All)
            {
                return _dbSet.AsQueryable();
            }
            var isDeleted = status == DeletedStatus.Deleted;
            return _dbSet.AsQueryable().Where(m => m.IsDeleted == isDeleted);
        }

        public IQueryable<T> NoTrackingSelect(DeletedStatus status = DeletedStatus.NotDeleted)
        {
            if (status == DeletedStatus.All)
            {
                return _dbSet.AsNoTracking().AsQueryable();
            }
            var isDeleted = status == DeletedStatus.Deleted;
            return _dbSet.AsNoTracking().AsQueryable().Where(m => m.IsDeleted == isDeleted);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.ToListAsync(cancellationToken: cancellationToken);
        }
        public async Task<IEnumerable<T>> GetAllWithDeletedAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(id, cancellationToken);
        }

        public async Task AddAsync(T obj, CancellationToken cancellationToken = default)
        {
            obj.CreatedUserId = userId!.Value;
            await _dbSet.AddAsync(obj,cancellationToken);
        }

        public void Update(T obj, CancellationToken cancellationToken = default)
        {
            obj.UpdatedAt = DateTime.UtcNow;
            obj.UpdatedById = userId;
            _dbSet.Update(obj);
        }

        public void Delete(T obj)
        {
            obj.IsDeleted = true;
            obj.DeletedAt = DateTime.UtcNow;
            obj.DeletedById = userId;
            _dbSet.Update(obj);
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await GetByIdAsync(id,cancellationToken) != null;
        }

       
    }
}