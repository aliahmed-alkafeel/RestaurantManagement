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
            var items = await unitOfWork.Items.Select().Where(i => model.ItemIds.Contains(i.Id)).ToListAsync(model.CancellationToken);
            discount.Items.AddRange(items);
            await unitOfWork.Discounts.AddAsync(discount, model.CancellationToken);
            await unitOfWork.SaveChangesAsync(model.CancellationToken);
            return true;
        }

        public async Task<List<DiscountViewModel>> GetAllDiscountsAsync(CancellationToken cancellationToken)
        {
            return await unitOfWork.Discounts.NoTrackingSelect().OrderBy(d => d.DiscountStartingDate)
                .Include(i => i.Items)
                .Select(discount => new DiscountViewModel
                {
                    Id = discount.Id,
                    DiscountPercentage = discount.DiscountPercentage,
                    DiscountStartingDate = discount.DiscountStartingDate,
                    DiscountEndingDate = discount.DiscountEndingDate,
                    Items = discount.Items
                }).ToListAsync(cancellationToken);
        }


        public async Task<DiscountViewModel> GetDiscountByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var discount = await unitOfWork.Discounts.Select().Where(d => d.Id == id)
                .Include(i => i.Items)
                .FirstOrDefaultAsync(cancellationToken);
            if (discount is null) throw new ArgumentNullException(nameof(discount));
            return new DiscountViewModel
            {
                Id = discount.Id,
                DiscountPercentage = discount.DiscountPercentage,
                DiscountStartingDate = discount.DiscountStartingDate,
                DiscountEndingDate = discount.DiscountEndingDate,
                Items = discount.Items
            };
             
        }

        public async Task<bool> DeleteDiscountAsync(Guid id, CancellationToken cancellationToken)
        {
            var discount = await unitOfWork.Discounts.GetByIdAsync(id, cancellationToken);
            if (discount is null) throw new InvalidOperationException("There is no such discount");
            unitOfWork.Discounts.Delete(discount);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> UpdateDiscountAsync(DiscountViewModel model)
        {
            if (model is null) throw new ArgumentNullException();
            var discount = await unitOfWork.Discounts.Select().
                Where(d => d.Id == model.Id).Include(i => i.Items)
                .FirstOrDefaultAsync(model.CancellationToken);
            if (discount is null) return false;
            discount.DiscountPercentage = model.DiscountPercentage;
            discount.DiscountEndingDate = model.DiscountEndingDate;
            discount.DiscountStartingDate = model.DiscountStartingDate;
            var existingItems = discount.Items.ToList();
            var toRemove = existingItems
                .Where(i => !model.ItemIds.Contains(i.Id)).ToHashSet();
            foreach (var oldItem in toRemove)
            {
                oldItem.Discount = null;
                unitOfWork.Items.Update(oldItem);
            }
            var items = await unitOfWork.Items.Select()
                .Where(i => model.ItemIds.Contains(i.Id)).ToHashSetAsync(model.CancellationToken);
            foreach (var item in items)
            {
                if (!discount.Items.Any(i => i.Id == item.Id)!) 
                {
                    discount.Items.Add(item);
                }
            }

            unitOfWork.Discounts.Update(discount);
            await unitOfWork.SaveChangesAsync(model.CancellationToken);
            return true;
        }
    }
}