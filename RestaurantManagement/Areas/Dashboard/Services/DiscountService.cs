using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            if (model is null) throw new ArgumentNullException(nameof(model));
            Discount discount = new Discount
            {
                Id = Guid.NewGuid(),
                DiscountPercentage = model.DiscountPercentage,
                DiscountStartingDate = model.DiscountStartingDate,
                DiscountEndingDate = model.DiscountEndingDate,
            };
            foreach(var itemId in model.ItemIds)
            {
                var item = await unitOfWork.Items.GetByIdAsync(itemId,model.CancellationToken);
                if (item is null) continue;
                discount.Items.Add(item);
            }
            await unitOfWork.Discounts.AddAsync(discount,model.CancellationToken);
            await unitOfWork.SaveChangesAsync(model.CancellationToken);
            return true;
        }

        public async Task<List<DiscountViewModel>> GetAllDiscountsAsync(CancellationToken cancellationToken)
        {
            var discounts = await unitOfWork.Discounts.GetAllDiscountsWithItemsAsync(cancellationToken);
            List<DiscountViewModel> discountsVm = [];
            foreach (Discount discount in discounts)
            {

                    discountsVm.Add(new DiscountViewModel
                    {
                        Id = discount.Id,
                        DiscountPercentage = discount.DiscountPercentage,
                        DiscountStartingDate = discount.DiscountStartingDate,
                        DiscountEndingDate = discount.DiscountEndingDate,
                        Items = discount.Items
                    });

            }          
            return discountsVm;
        }

        public async Task<DiscountViewModel> GetDiscountByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var discount = await unitOfWork.Discounts.GetDiscountWithItemsByIdAsync(id,cancellationToken);
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

        public async Task<bool> DeleteDiscountAsync(Guid modelId, CancellationToken cancellationToken)
        {
            var discount = await unitOfWork.Discounts.GetByIdAsync(modelId,cancellationToken);
            if (discount is null) throw new InvalidOperationException("There is no such discount");
            unitOfWork.Discounts.Delete(discount);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> UpdateDiscountAsync(DiscountViewModel model)
        {
            if (model is null) throw new ArgumentNullException();
            var discount = await unitOfWork.Discounts.GetDiscountWithItemsByIdAsync(model.Id, model.CancellationToken);
            if (discount is null) return false;
            discount.DiscountPercentage = model.DiscountPercentage;
            discount.DiscountEndingDate = model.DiscountEndingDate;
            discount.DiscountStartingDate = model.DiscountStartingDate;
            var existingItems = discount.Items.ToList();
            var toRemove = existingItems
                .Where(i => !model.ItemIds.Contains(i.Id));
            foreach (var oldItem in toRemove)
            {
                oldItem.Discount = null;
                unitOfWork.Items.Update(oldItem);
            }
            
            foreach (var itemId in model.ItemIds)
            {
                var item = await unitOfWork.Items.GetByIdAsync(itemId, model.CancellationToken);
                if (item is null) continue;
                discount.Items.Add(item);
            }
            unitOfWork.Discounts.Update(discount);
            await unitOfWork.SaveChangesAsync(model.CancellationToken);
            return true;
        }
    }
}

