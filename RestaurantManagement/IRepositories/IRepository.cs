using RestaurantManagement.Models;

namespace RestaurantManagement.IRepositories
{
    public enum DeletedStatus
    {
        All,
        Deleted,
        NotDeleted
    }
    public interface IRepository<T>
    {
        IQueryable<T> Select(DeletedStatus status = DeletedStatus.NotDeleted);
        IQueryable<T> NoTrackingSelect(DeletedStatus status = DeletedStatus.NotDeleted);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAllWithDeletedAsync(CancellationToken cancellationToken = default);
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(T obj, CancellationToken cancellationToken = default);
        void Update(T obj);
        void Delete(T obj);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
