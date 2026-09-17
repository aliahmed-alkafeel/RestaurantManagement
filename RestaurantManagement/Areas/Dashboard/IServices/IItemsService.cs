using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.Models;

namespace RestaurantManagement.Areas.Dashboard.IServices
{
    public interface IItemsService
    {
        Task<List<ItemViewModel>> GetAllItemsAsync(CancellationToken cancellationToken = default);
        Task<ItemViewModel> GetItemByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> CreateItemAsync(ItemViewModel model);
        Task<bool> UpdateItemAsync(ItemViewModel model);
        Task<bool> DeleteItemAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<ItemByCategoryViewModel>> GetItemsByType(CategoryType type, CancellationToken cancellationToken = default);
        Task<IEnumerable<Category>> GetCategoriesByTypeAsync(CategoryType type, CancellationToken cancellationToken = default);
        public Task<List<ItemByCategoryViewModel>> GetItemsByCategoryId(Guid categoryId, CancellationToken cancellationToken = default);
        public Task<ItemsPageViewModel> GetPagedItemsAsync(ItemFilterViewModel model);
    }
}