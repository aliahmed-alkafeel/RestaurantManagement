using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
        private readonly DbSet<T> _dbSet;
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


        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default, DeletedStatus status = DeletedStatus.NotDeleted)
        {
            if (status == DeletedStatus.All)
            {
                return await _dbSet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            }
            var isDeleted = status == DeletedStatus.Deleted;
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == isDeleted, cancellationToken);
        }

        public async Task AddAsync(T obj, CancellationToken cancellationToken = default)
        {
            obj.CreatedUserId = userId!.Value;
            obj.CreatedAt = DateTime.UtcNow;
            await _dbSet.AddAsync(obj,cancellationToken);
        }

        public void DeleteRange(IEnumerable<T> objects)
        {
            var deletedAt = DateTime.UtcNow;

            foreach (var obj in objects)
            {
                obj.IsDeleted = true;
                obj.DeletedAt = deletedAt;
                obj.DeletedById = userId;
            }

            _dbSet.UpdateRange(objects);
        }
        public void Update(T obj)
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

        //public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default,
        //    DeletedStatus status = DeletedStatus.NotDeleted)
        //{
        //    return await GetByIdAsync(id, cancellationToken, status) != null;
        //}




    }
}