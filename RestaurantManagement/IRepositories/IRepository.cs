using RestaurantManagement.Models;

namespace RestaurantManagement.IRepositories
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAllWithDeletedAsync(CancellationToken cancellationToken = default);
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(T obj, CancellationToken cancellationToken = default);
        void Update(T obj,Guid UpdatedById, CancellationToken cancellationToken = default);
        void Delete(T obj, Guid DeletedById, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
