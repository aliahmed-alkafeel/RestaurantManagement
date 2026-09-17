using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.Models;

namespace RestaurantManagement.Areas.Dashboard.IServices
{
    public interface IItemsService
    {
        Task<List<ItemViewModel>> GetAllItemsAsync();
        Task<ItemViewModel> GetItemByIdAsync(Guid Id);
        Task<bool> CreateItemAsync(ItemViewModel model);
        Task<bool> UpdateItemAsync(ItemViewModel model);
        Task<bool> DeleteItemAsync(Guid id);
        Task<List<ItemByCategoryViewModel>> GetItemsByType(CategoryType type);
        Task<IEnumerable<Category>> GetCategoriesByTypeAsync(CategoryType type);
        public Task<List<ItemByCategoryViewModel>> GetItemsByCategoryId(Guid categoryId);
        public Task<ItemsPageViewModel> GetPagedItemsAsync(ItemFilterViewModel model);
    }
}