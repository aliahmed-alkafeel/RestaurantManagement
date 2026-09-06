using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Areas.Dashboard.IServices;
using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.IRepositories;
using RestaurantManagement.Migrations;
using RestaurantManagement.Models;
using RestaurantManagement.Repositories;
using System.Linq;

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
            var items = await unitOfWork.Items.NoTrackingSelect().Include(i => i.Category).Include(i => i.Discount).ToListAsync();
            List<ItemViewModel> itemsVm = [];
            foreach (Item item in items)
            {
            //Console.WriteLine(item.Discount != null ?item.Discount.DiscountPercentage: "not");
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
                        ImageUrl = item.ImageUrl,
                        DiscountPercentage = item.Discount != null &&
                        item.Discount.DiscountStartingDate <= DateTime.UtcNow && item.Discount.DiscountEndingDate >= DateTime.UtcNow
                        ? item.Discount.DiscountPercentage : null
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
            if (model.ItemImage == null || model.ItemImage.Length == 0)
            {
                return true;
            }
            if (model.ItemImage.Length > maxLength)
            {
                return false;
            }
            var allowedExtensions = new[]
            {
                ".jpg",".jpeg",".png",".webp",".gif"
            };
            var extension = Path.GetExtension(model.ItemImage.FileName);
            if (!allowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase)) return false;
            if (!model.ItemImage.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase)) return false;
            var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "items");
            string fileName = model.ItemName+"_"+ new Guid() + extension;            
            string filePath = Path.Combine(directory, fileName);
            await using var stream = new FileStream(filePath, FileMode.Create);
            await model.ItemImage.CopyToAsync(stream);
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
        public async Task<List<ItemByCategoryViewModel>> GetItemsByCategoryId(Guid categoryId)
        {
            return await unitOfWork.Items.Select().Where(i => i.CategoryId == categoryId)
                .Select(i => new ItemByCategoryViewModel
                {
                    Id = i.Id,
                    ItemName = i.ItemName,
                    Price = i.Price,
                    Image = i.ImageUrl,
                    DiscountPercentage = i.Discount != null &&
                        i.Discount.DiscountStartingDate <= DateTime.UtcNow && i.Discount.DiscountEndingDate >= DateTime.UtcNow
                        ? i.Discount.DiscountPercentage : null
                }).ToListAsync();
        }

        public async Task<ItemsPageViewModel> GetPagedItemsAsync(ItemFilterViewModel model)
        {
            if (model.Page < 1) model.Page = 1;
            if (model.PageSize < 1) model.PageSize = 10;

            var items = unitOfWork.Items.NoTrackingSelect().Include(x => x.Category).AsNoTracking();
            var categories = await unitOfWork.Categories.NoTrackingSelect().ToListAsync();

            if (!string.IsNullOrEmpty(model.Search))
            {
                items = items.Where(x => x.ItemName.Contains(model.Search));
            }
            if(model.Type is not null)
            {
                items = items.Where(x => x.Category.Type == model.Type.Value);
                categories = categories.Where(x => x.Type == model.Type.Value).ToList();
            }
            if(model.CategoryId is not null)
            {
                items = items.Where(x => x.CategoryId == model.CategoryId.Value);
            }
            if(model.MinPrice is not null)
            {
                items = items.Where(x => x.Price >= model.MinPrice.Value);
            }
            if(model.MaxPrice is not null)
            {
                items = items.Where(x => x.Price <= model.MaxPrice.Value);
            }
            if(model.IsAvailable is not null)
            {
                items = items.Where(x => x.IsAvailable == model.IsAvailable);
            }
            items = model.Sort switch
            {
                "name_asc" => items.OrderBy(x => x.ItemName),
                "name_desc" => items.OrderByDescending(x => x.ItemName),
                "price_asc" => items.OrderBy(x => x.Price),
                "price_desc" => items.OrderByDescending(x => x.Price),
                "isActive_asc" => items.OrderBy(x => !x.IsActive),
                "isActive_desc" => items.OrderByDescending(x => !x.IsActive),
                "isAvailable_asc" => items.OrderBy(x => !x.IsAvailable),
                "isAvailable_desc" => items.OrderByDescending(x => !x.IsAvailable),
                "category_asc" => items.OrderBy(x => x.Category.CategoryName),
                "category_desc" => items.OrderByDescending(x => x.Category.CategoryName),

                _ => items.OrderBy(x => x.IsActive).OrderBy(x => x.ItemName)
            };
            var totalCount = await items.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount /(double) model.PageSize);
            var finalItems = await items.Skip((model.Page - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            var result = new PaginatedList<Item>
            {
                Items = finalItems,
                CurrentPage = model.Page,
                PageSize = model.PageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };
            return new ItemsPageViewModel
            {
                Items = result,
                Filter = model,
                Categories = categories
            };
        }
    }
    }