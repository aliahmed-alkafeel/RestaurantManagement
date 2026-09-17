using Microsoft.AspNetCore.Mvc;
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
            if (model is null) throw new ArgumentNullException();
            var categories = await unitOfWork.Categories.GetAllAsync(model.CancellationToken);
            foreach(Category cat in categories)
            {
                if(cat.CategoryName == model.CategoryName && cat.Type == model.Type)
                {
                    return false;
                }
            }
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
            var categories = await unitOfWork.Categories.GetAllAsync(cancellationToken);
            List<CategoryViewModel> categoriesVm = [];
            foreach (Category category in categories)
            {
                if (!category.IsDeleted)
                {
                    categoriesVm.Add(new CategoryViewModel
                    {
                        Id = category.Id,
                        CategoryName = category.CategoryName,
                        Type = category.Type
                    });
                }
            }
            return categoriesVm;
        }

        public async Task<CategoryViewModel> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var category = await unitOfWork.Categories.GetByIdAsync(id,cancellationToken);
            if (category is null) throw new KeyNotFoundException("There is no such category");
            CategoryViewModel categoryVm = new CategoryViewModel
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
                Type = category.Type
            };
            return categoryVm;

        }

        public async Task<bool> DeleteCategoryAsync(Guid id, CancellationToken cancellationToken)
        {
            var category = await unitOfWork.Categories.GetByIdAsync(id,cancellationToken);
            if (category is null) throw new InvalidOperationException("There is no such category");
            unitOfWork.Categories.Delete(category);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> UpdateCategoryAsync(CategoryViewModel model)
        {
            if (model is null) throw new ArgumentNullException();
            var categories = await unitOfWork.Categories.GetAllAsync(model.CancellationToken);
            foreach (Category cat in categories)
            {
                if ((cat.CategoryName == model.CategoryName && cat.Id != model.Id) &&
                    (cat.Type == model.Type && cat.Id != model.Id))
                {
                    return false;
                }
            }
            var category = categories.FirstOrDefault(c => c.Id == model.Id);
            if (category is null) return false;
            category.CategoryName = model.CategoryName;
            category.Type = model.Type;
            unitOfWork.Categories.Update(category);
            await unitOfWork.SaveChangesAsync(model.CancellationToken);
            return true;
        }


    }
}
