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
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default, DeletedStatus status = DeletedStatus.NotDeleted);
        Task AddAsync(T obj, CancellationToken cancellationToken = default);
        void Update(T obj);
        void Delete(T obj);
        //Task<bool> ExistsByIdAsync(Guid id, DeletedStatus status = DeletedStatus.NotDeleted, CancellationToken cancellationToken = default);
    }
}
