using RestaurantManagement.Models;
using RestaurantManagement.ViewModels;

namespace RestaurantManagement.IServices
{
    public interface IDashboardService<T>
    {
        Task<List<T>> GetAllAsync();
        Task UpdateAsync(T t);
        Task DeleteAsync(T t);
    }
}
