using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.Models;

namespace RestaurantManagement.Areas.Dashboard.IServices
{
    public interface IItemsService
    {
        Task<List<ItemViewModel>> GetAllItemsAsync();
        Task<ItemViewModel> GetItemByIdAsync(Guid Id);
        Task<bool> CreateItemAsync(ItemViewModel model);
        Task<bool> UpdateItemAsync(ItemViewModel model, Guid ModifierId);
        Task<bool> DeleteItemAsync(Guid id, Guid ModifierId);
        Task<IEnumerable<Category>> GetCategoriesByTypeAsync(CategoryType type);

    }
}