using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Areas.Dashboard.IServices;
using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.IRepositories;
using RestaurantManagement.Models;
using RestaurantManagement.Repositories;

namespace RestaurantManagement.Areas.Dashboard.Services
{
    public class DiscountService(IUnitOfWork unitOfWork) : IDiscountService
    {

        public async Task<bool> CreateDiscountAsync(DiscountViewModel model)
        {
            if (model is null) throw new ArgumentNullException();
            Discount discount = new Discount
            {
                Id = Guid.NewGuid(),
                DiscountPercentage = model.DiscountPercentage,
                DiscountStartingDate = model.DiscountStartingDate,
                DiscountEndingDate = model.DiscountEndingDate,
            };
            foreach(var itemId in model.ItemIds)
            {
                var item = await unitOfWork.Items.GetByIdAsync(itemId);
                if (item is null) continue;
                discount.Items.Add(item);
            }
            await unitOfWork.Discounts.AddAsync(discount);
            await unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<List<DiscountViewModel>> GetAllDiscountsAsync()
        {
            var discounts = await unitOfWork.Discounts.GetAllDiscountsWithItemsAsync();
            List<DiscountViewModel> discountsVm = [];
            foreach (Discount discount in discounts)
            {

                    discountsVm.Add(new DiscountViewModel
                    {
                        Id = Guid.NewGuid(),
                        DiscountPercentage = discount.DiscountPercentage,
                        DiscountStartingDate = discount.DiscountStartingDate,
                        DiscountEndingDate = discount.DiscountEndingDate,
                        Items = discount.Items
                    });
                }          
            return discountsVm;
        }

        public async Task<DiscountViewModel> GetDiscountByIdAsync(Guid Id)
        {
            var discount = await unitOfWork.Discounts.GetDiscountWithItemsByIdAsync(Id);
            if (discount is null) throw new KeyNotFoundException("There is no such discount");
            DiscountViewModel discountVm = new DiscountViewModel
            {
                Id = discount.Id,
                DiscountPercentage = discount.DiscountPercentage,
                DiscountStartingDate = discount.DiscountStartingDate,
                DiscountEndingDate = discount.DiscountEndingDate,
                Items = discount.Items
            };
            return discountVm;

        }

        public async Task<bool> DeleteDiscountAsync(Guid modelId, Guid ModifierId)
        {
            var discount = await unitOfWork.Discounts.GetByIdAsync(modelId);
            if (discount is null) throw new InvalidOperationException("There is no such discount");
            unitOfWork.Discounts.Delete(discount, ModifierId);
            await unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateDiscountAsync(DiscountViewModel model, Guid ModifierId)
        {
            if (model is null) throw new ArgumentNullException();
            var discount = await unitOfWork.Discounts.GetDiscountWithItemsByIdAsync(model.Id);
            if (discount is null) return false;
            discount.DiscountPercentage = model.DiscountPercentage;
            discount.DiscountEndingDate = model.DiscountEndingDate;
            discount.DiscountStartingDate = model.DiscountStartingDate;
            discount.Items = discount.Items;
            unitOfWork.Discounts.Update(discount, ModifierId);
            await unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}

