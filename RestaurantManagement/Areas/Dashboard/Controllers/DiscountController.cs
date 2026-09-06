using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Areas.Dashboard.IServices;
using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.Models;
using System.Security.Claims;

namespace RestaurantManagement.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Route("[area]/[controller]")]
        public class DiscountsController(IDiscountService discountsService) : Controller
        {
            [Authorize(Roles = nameof(UserRole.AccessDiscounts))]
            [HttpGet("")]
            public async Task<IActionResult> Discounts()
            {
                var discounts = await discountsService.GetAllDiscountsAsync();
                return View(discounts);
            }

            [Authorize(Roles = nameof(UserRole.ManageDiscounts))]
            [HttpGet("CreateDiscount")]
            public async Task<IActionResult> CreateDiscount()
            {
                return View("ManageDiscount",new DiscountViewModel());
            }

            [Authorize(Roles = nameof(UserRole.ManageDiscounts))]
            [HttpPost("CreateDiscount")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> CreateDiscount(DiscountViewModel model)
            {
                if (!ModelState.IsValid) return View(model);
                var result = await discountsService.CreateDiscountAsync(model);
                if (result is false)
                {
                    ModelState.AddModelError("", "The Discount is Regestered");
                    return View(model);
                }
                return RedirectToAction(nameof(Discounts));
            }
            [Authorize(Roles = nameof(UserRole.ManageDiscounts))]
            [HttpGet("EditDiscount/{id:guid}")]
            public async Task<IActionResult> EditDiscount(Guid id)
            {
                var discount = await discountsService.GetDiscountByIdAsync(id);
                return View("ManageDiscount", discount);
            }
            [Authorize(Roles = nameof(UserRole.ManageDiscounts))]
            [HttpPost("EditDiscount/{id:guid}")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> EditDiscount(DiscountViewModel model)
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var result = await discountsService.UpdateDiscountAsync(model, Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));
                if (!result)
                {
                    ModelState.AddModelError("", "This update is not allowed");
                    return View(model);
                }
                return RedirectToAction(nameof(Discounts));
            }

            [Authorize(Roles = nameof(UserRole.ManageDiscounts))]
            [HttpGet("DeleteDiscount/{id:guid}")]
            public async Task<IActionResult> DeleteDiscount(Guid id)
            {
                var emps = await discountsService.GetDiscountByIdAsync(id);
                return View(emps);
            }
            [ValidateAntiForgeryToken]
            [Authorize(Roles = nameof(UserRole.ManageDiscounts))]
            [HttpPost("ConfirmedDeleteDiscount/{id:guid}")]
            public async Task<IActionResult> ConfirmedDeleteDiscount(Guid id)
            {
                await discountsService.DeleteDiscountAsync(id, Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));
                return RedirectToAction(nameof(Discounts));
            }

        }
    }
