using RestaurantManagement.Models;

namespace RestaurantManagement.IRepository
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetById(int id);
        Task AddAsync(T obj);
        void Update(T obj);
        void Delete(T obj);
        Task<bool> ExistsAsync(int id);


    }
}
