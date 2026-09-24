using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Areas.Dashboard.IServices;
using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.Models;
using System.Security.Claims;
using RestaurantManagement.Extensions;

namespace RestaurantManagement.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Route("[area]/[controller]")]
        public class DiscountsController(IDiscountService discountsService) : BaseController
    {
            [Authorize(Roles = nameof(UserRole.AccessDiscounts))]
            [HttpGet("")]
            public async Task<IActionResult> Discounts(CancellationToken cancellationToken)
            {
                var discounts = await discountsService.GetAllDiscountsAsync(cancellationToken);
                return View(discounts);
            }

            [Authorize(Roles = nameof(UserRole.ManageDiscounts))]
            [HttpGet("CreateDiscount")]
            public IActionResult CreateDiscount()
            {
                return View("ManageDiscount",new DiscountViewModel());
            }

            [Authorize(Roles = nameof(UserRole.ManageDiscounts))]
            [HttpPost("CreateDiscount")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> CreateDiscount(DiscountViewModel model)
            {
                if (!ModelState.IsValid) return View("ManageDiscount",model);
                var result = await discountsService.CreateDiscountAsync(model);
                if (result is false)
                {
                    ModelState.AddModelError("", "The Discount is Registered");
                    return View("ManageDiscount",model);
                }
                TempData.SuccessMessage(
                    NotificationExtensions.ActionType.Create,
                    NotificationExtensions.EntityType.Discount);
            return RedirectToAction(nameof(Discounts));
            }
            [Authorize(Roles = nameof(UserRole.ManageDiscounts))]
            [HttpGet("EditDiscount/{id:guid}")]
            public async Task<IActionResult> EditDiscount(Guid id,CancellationToken cancellationToken)
            {
                var discount = await discountsService.GetDiscountByIdAsync(id, cancellationToken);
                return View("ManageDiscount", discount);
            }
            [Authorize(Roles = nameof(UserRole.ManageDiscounts))]
            [HttpPost("EditDiscount/{id:guid}")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> EditDiscount(DiscountViewModel model)
            {
                if (!ModelState.IsValid)
                {
                    return View("ManageDiscount",model);
                }
                var result = await discountsService.UpdateDiscountAsync(model);
                if (!result)
                {
                    ModelState.AddModelError("", "This update is not allowed");
                    return View("ManageDiscount",model);
                }
                TempData.SuccessMessage(
                    NotificationExtensions.ActionType.Update,
                    NotificationExtensions.EntityType.Discount);
            return RedirectToAction(nameof(Discounts));
            }

            [Authorize(Roles = nameof(UserRole.ManageDiscounts))]
            [HttpGet("DeleteDiscount/{id:guid}")]
            public async Task<IActionResult> DeleteDiscount(Guid id,CancellationToken cancellationToken)
            {
                var discount = await discountsService.GetDiscountByIdAsync(id, cancellationToken);
                return View(discount);
            }
            [ValidateAntiForgeryToken]
            [Authorize(Roles = nameof(UserRole.ManageDiscounts))]
            [HttpPost("ConfirmedDeleteDiscount/{id:guid}")]
            public async Task<IActionResult> ConfirmedDeleteDiscount(Guid id,CancellationToken cancellationToken)
            {
                await discountsService.DeleteDiscountAsync(id, cancellationToken);
                //if (!result)
                //{
                //    var category = await discountsService.GetDiscountByIdAsync(id, cancellationToken);
                //    ModelState.AddModelError(string.Empty, "The category could not be deleted.");
                //    return View("DeleteDiscount", category);
                //}
            TempData.SuccessMessage(
                    NotificationExtensions.ActionType.Delete,
                    NotificationExtensions.EntityType.Discount);
            return RedirectToAction(nameof(Discounts));
            }

        }
    }
