using Microsoft.EntityFrameworkCore;
using RestaurantManagement.IRepositories;
using RestaurantManagement.IServices;
using RestaurantManagement.Models;
using RestaurantManagement.ViewModels;

namespace RestaurantManagement.Services
{
    public class ItemAvailabilityService(IUnitOfWork unitOfWork) : IItemAvailabilityService
    {

        public async Task<ItemAvailabilityViewModel> GetAvailabilityAsync(
            CancellationToken cancellationToken = default)
        {
            var items = await unitOfWork.Items
                .NoTrackingSelect()
                .Include(x => x.Category)
                .OrderBy(x => x.Category.Type)
                .ThenBy(x => x.Category.CategoryName)
                .ThenBy(x => x.ItemName)
                .ToListAsync(cancellationToken);


            return new ItemAvailabilityViewModel
            {
                AvailableGroups = BuildGroups(
                    items.Where(x => x.IsAvailable)
                ),

                UnavailableGroups = BuildGroups(
                    items.Where(x => !x.IsAvailable)
                )
            };
        }



        public async Task<AvailabilityItemViewModel?> ToggleAvailabilityAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var item = await unitOfWork.Items
                .Select()
                .Include(x => x.Category)
                .FirstOrDefaultAsync(
                    x =>
                        x.Id == id &&
                        !x.IsDeleted,
                    cancellationToken);


            if (item is null)
                return null;


            item.IsAvailable = !item.IsAvailable;


            await unitOfWork.SaveChangesAsync(cancellationToken);


            return MapItem(item);
        }

        private static List<ItemAvailabilityTypeViewModel> BuildGroups(
            IEnumerable<Item> items)
        {
            return items
                .GroupBy(x => x.Category.Type)
                .OrderBy(x => x.Key)
                .Select(typeGroup => new ItemAvailabilityTypeViewModel
                {
                    Type = typeGroup.Key,

                    Categories = typeGroup
                        .GroupBy(x => x.CategoryId)
                        .OrderBy(x => x.First().Category.CategoryName)
                        .Select(categoryGroup =>
                        {
                            var category = categoryGroup.First().Category;

                            return new ItemAvailabilityCategoryViewModel
                            {
                                CategoryId = category.Id,

                                CategoryName = category.CategoryName,

                                Items = categoryGroup
                                    .OrderBy(x => x.ItemName)
                                    .Select(MapItem)
                                    .ToList()
                            };
                        })
                        .ToList()
                })
                .ToList();
        }

        private static AvailabilityItemViewModel MapItem(
            Item item)
        {
            return new AvailabilityItemViewModel
            {
                Id = item.Id,

                ItemName = item.ItemName,

                Price = item.Price,

                IsAvailable = item.IsAvailable,

                CategoryId = item.CategoryId,

                CategoryName =
                    item.Category.CategoryName,

                Type =
                    item.Category.Type,
                ImageUrl = string.IsNullOrWhiteSpace(item.ImageUrl)
                    ? "/images/items/default.jpg"
                    : item.ImageUrl.StartsWith("~/")
                        ? item.ImageUrl[1..]
                        : item.ImageUrl
            };
        }
    
}
}
