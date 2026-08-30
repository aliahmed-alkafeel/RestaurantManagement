using Microsoft.AspNetCore.Http.HttpResults;
using RestaurantManagement.Areas.Dashboard.IServices;
using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.IRepositories;
using RestaurantManagement.Models;
using System.Linq;
using RestaurantManagement.Repositories;

namespace RestaurantManagement.Areas.Dashboard.Services
{
    public class ItemsService(IUnitOfWork unitOfWork) : IItemsService
    {
        public async Task<bool> CreateItemAsync(ItemViewModel model)
        {
            if (model is null) throw new ArgumentNullException();
            var items = await unitOfWork.Items.GetAllAsync();
            foreach (Item i in items)
            {
                if (i.ItemName == model.ItemName && i.CategoryId == model.CategoryId)
                {
                    return false;
                }
            }
            var imageResult = await InsertImage(model);
            if (!imageResult) return false;
            var category = await unitOfWork.Categories.GetByIdAsync(model.CategoryId);
            Item item = new Item
            {
                Id = Guid.NewGuid(),
                ItemName = model.ItemName,
                CategoryId = model.CategoryId,
                Category = category!,
                Price = model.Price,
                IsActive = model.IsActive,
                IsAvailable = model.IsAvailable,
                ImageUrl = model.ImageUrl
            };
            await unitOfWork.Items.AddAsync(item);
            await unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteItemAsync(Guid modelId, Guid ModifierId)
        {
            var item = await unitOfWork.Items.GetByIdAsync(modelId);
            if (item is null) throw new InvalidOperationException("There is no such Item");
            DeleteImage(item); 
            unitOfWork.Items.Delete(item, ModifierId);
            await unitOfWork.SaveChangesAsync();
            return true;
        }
        public void DeleteImage(Item item)
        {
            if (!item.ImageUrl.EndsWith("default.jpg"))
            {
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", item.ImageUrl.TrimStart('~', '/'));
                if (File.Exists(oldPath))
                {
                    var newPath = Path.Combine(Path.GetDirectoryName(oldPath)!, $"deleted_{item.ItemName}_{DateTime.UtcNow:yyyyMMddHHmmss}{Path.GetExtension(oldPath)}");
                    File.Move(oldPath, newPath);
                }
            }
        }
        public async Task<List<ItemViewModel>> GetAllItemsAsync()
        {
            var items = await unitOfWork.Items.GetAllAsync();
            List<ItemViewModel> itemsVm = [];
            foreach (Item item in items)
            {
                if (!item.IsDeleted)
                {
                    var category = await unitOfWork.Categories.GetByIdAsync(item.CategoryId);
                    itemsVm.Add(new ItemViewModel
                    {
                        Id = item.Id,
                        ItemName = item.ItemName,
                        Price = item.Price,
                        IsActive = item.IsActive,
                        IsAvailable = item.IsAvailable,
                        Category = category,
                        CategoryId = item.CategoryId,
                        ImageUrl = item.ImageUrl
                    });
                }
            }
            return itemsVm;
        }

        public async Task<ItemViewModel> GetItemByIdAsync(Guid Id)
        {
            var item = await unitOfWork.Items.GetByIdAsync(Id);
            if (item is null) throw new KeyNotFoundException("There is no such Item");
            var category = await unitOfWork.Categories.GetByIdAsync(item.CategoryId);
            ItemViewModel ItemVm = new ItemViewModel
            {
                Id = item.Id,
                ItemName = item.ItemName,
                Category = category!,
                Price = item.Price,
                IsActive = item.IsActive,
                IsAvailable = item.IsAvailable
            };
            return ItemVm;
        }

        public async Task<bool> InsertImage(ItemViewModel model)
        {
            var maxLength = 30 * 1024 * 1024;
            if (model.Image == null || model.Image.Length == 0)
            {
                return true;
            }
            if (model.Image.Length > maxLength)
            {
                return false;
            }
            var allowedExtensions = new[]
            {
                ".jpg",".jpeg",".png",".webp",".gif"
            };
            var extension = Path.GetExtension(model.Image.FileName);
            if (!allowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase)) return false;
            if (!model.Image.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase)) return false;
            var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "items");
            string fileName = model.ItemName+"_"+ new Guid() + extension;            
            string filePath = Path.Combine(directory, fileName);
            await using var stream = new FileStream(filePath, FileMode.Create);
            await model.Image.CopyToAsync(stream);
            model.ImageUrl = $"~/images/items/{fileName}";
            return true;
        }

        public async Task<bool> UpdateItemAsync(ItemViewModel model, Guid ModifierId)
        {
            if (model is null) throw new ArgumentNullException();
            var Items = await unitOfWork.Items.GetAllAsync();
            foreach (Item i in Items)
            {
                if ((i.ItemName == model.ItemName && i.Id != model.Id) &&
                    (i.CategoryId == model.CategoryId && i.Id != model.Id))
                {
                    return false;
                }
            }
            var item = Items.FirstOrDefault(c => c.Id == model.Id);
            if (item is null) return false;
            if (!await InsertImage(model)) return false;
            DeleteImage(item);
            Category? category = await unitOfWork.Categories.GetByIdAsync(model.CategoryId);
            item.ItemName = model.ItemName;
            item.Category = category!;
            item.Price = model.Price;
            item.IsActive = model.IsActive;
            item.IsAvailable = model.IsAvailable;
            item.ImageUrl = model.ImageUrl;
            unitOfWork.Items.Update(item, ModifierId);
            await unitOfWork.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Category>> GetCategoriesByTypeAsync(CategoryType type)
        {
            return (await unitOfWork.Categories.GetAllAsync()).Where(c => c.Type == type);
        }
        public async Task<List<Item>> GetItemsByCategoryId(Guid categoryId)
        {
            return (await unitOfWork.Items.GetItemsWithCategories()).Where(i => i.CategoryId == categoryId).ToList();
        }
    }
    }