using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.Models;

namespace RestaurantManagement.Areas.Dashboard.IServices
{
    public interface IOrdersService
    {
        Task<List<OrderViewModel>> GetAllOrdersAsync();
        Task<OrderViewModel> GetOrderByIdAsync(Guid Id);
        Task<List<OrderViewModel>> OrderDetailsAsync(OrderViewModel model);
        Task<bool> UpdateOrderAsync(OrderViewModel model, Guid ModifierId);
        Task<bool> DeleteOrderAsync(Guid id, Guid ModifierId);
    }
}
