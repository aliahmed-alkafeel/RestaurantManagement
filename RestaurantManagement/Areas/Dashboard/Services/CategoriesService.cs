using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Areas.Dashboard.IServices;
using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.IRepositories;
using RestaurantManagement.Models;
using RestaurantManagement.Repositories;

namespace RestaurantManagement.Areas.Dashboard.Services
{
    public class CategoriesService(IUnitOfWork unitOfWork) : ICategoriesService
    {

        public async Task<bool> CreateCategoryAsync(CategoryViewModel model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));
            var isCategory = await unitOfWork.Categories.NoTrackingSelect().FirstOrDefaultAsync(c =>
                c.CategoryName == model.CategoryName && c.Type == model.Type, model.CancellationToken);
            if (isCategory != null) return false;
            Category category = new Category
            {
                Id = Guid.NewGuid(),
                CategoryName = model.CategoryName,
                Type = model.Type
            };
            await unitOfWork.Categories.AddAsync(category, model.CancellationToken);
            await unitOfWork.SaveChangesAsync(model.CancellationToken);
            return true;
        }

        public async Task<List<CategoryViewModel>> GetAllCategoriesAsync(CancellationToken cancellationToken)
        {
            return await unitOfWork.Categories.NoTrackingSelect().Select(c =>
                new CategoryViewModel()
                {
                    Id = c.Id,
                    CategoryName = c.CategoryName,
                    Type = c.Type
                }
            ).ToListAsync(cancellationToken);
        }

        public async Task<CategoryViewModel> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var category = await unitOfWork.Categories.GetByIdAsync(id,cancellationToken);
            if (category is null) throw new ArgumentNullException(nameof(category));
            return new CategoryViewModel
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
                Type = category.Type
            };
        }

        public async Task<bool> DeleteCategoryAsync(Guid id, CancellationToken cancellationToken)
        {
            var category = await unitOfWork.Categories.GetByIdAsync(id,cancellationToken);
            if (category is null) throw new ArgumentNullException(nameof(category));


            var hasItems = await unitOfWork.Items
                .NoTrackingSelect()
                .AnyAsync(i => i.CategoryId == id, cancellationToken);

            if (hasItems)
                return false;
            unitOfWork.Categories.Delete(category);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> UpdateCategoryAsync(CategoryViewModel model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));
            var category = await unitOfWork.Categories.Select()
                .FirstOrDefaultAsync(c => c.Id == model.Id, model.CancellationToken);
            if (category is null) return false;
            if (await unitOfWork.Categories.NoTrackingSelect()
                    .AnyAsync(c => c.Id != model.Id &&
                                   c.CategoryName == model.CategoryName &&
                                   c.Type == model.Type,
                        model.CancellationToken))
                return false;
            category.CategoryName = model.CategoryName;
            category.Type = model.Type;
            unitOfWork.Categories.Update(category);
            await unitOfWork.SaveChangesAsync(model.CancellationToken);
            return true;
        }
    }
}
