using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RestaurantManagement.Areas.Dashboard.IServices;
using RestaurantManagement.Areas.Dashboard.Services;
using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.Models;
using System.Security.Claims;
using RestaurantManagement.Extensions;

namespace RestaurantManagement.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Route("[area]/[controller]")]
    public class CategoriesController(ICategoriesService categoriesService) : Controller
    {
        [Authorize(Roles = nameof(UserRole.AccessCategories))]
        [HttpGet("")]
        public async Task<IActionResult> Categories(CancellationToken cancellationToken)
        {
            var categories = await categoriesService.GetAllCategoriesAsync(cancellationToken);
            return View(categories);
        }


        [Authorize(Roles = nameof(UserRole.ManageCategories))]
        [HttpGet("CreateCategory")]
        public async Task<IActionResult> CreateCategory()
        {
            return View();
        }

        [Authorize(Roles = nameof(UserRole.ManageCategories))]
        [HttpPost("CreateCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(CategoryViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await categoriesService.CreateCategoryAsync(model);
            if (result is false)
            {
                ModelState.AddModelError("", "The Category is Regestered");
                return View(model);
            }
            TempData.SuccessMessage(
                NotificationExtensions.ActionType.Create,
                NotificationExtensions.EntityType.Category);
            return RedirectToAction(nameof(Categories));
        }
        [Authorize(Roles = nameof(UserRole.ManageCategories))]
        [HttpGet("EditCategory/{id:guid}")]
        public async Task<IActionResult> EditCategory(Guid id, CancellationToken cancellationToken)
        {
            var order = await categoriesService.GetCategoryByIdAsync(id, cancellationToken);
            return View(order);
        }
        [Authorize(Roles = nameof(UserRole.ManageCategories))]
        [HttpPost("EditCategory/{id:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await categoriesService.UpdateCategoryAsync(model);
            if (!result)
            {
                ModelState.AddModelError("", "This update is not allowed");
                return View(model);
            }
            TempData.SuccessMessage(
                NotificationExtensions.ActionType.Update,
                NotificationExtensions.EntityType.Category);
            return RedirectToAction(nameof(Categories));
        }

        [Authorize(Roles = nameof(UserRole.ManageCategories))]
        [HttpGet("DeleteCategory/{id:guid}")]
        public async Task<IActionResult> DeleteCategory(Guid id, CancellationToken cancellationToken)
        {
            var category = await categoriesService.GetCategoryByIdAsync(id, cancellationToken);
            return View(category);
        }
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UserRole.ManageCategories))]
        [HttpPost("ConfirmedDeleteCategory/{id:guid}")]
        public async Task<IActionResult> ConfirmedDeleteCategory(Guid id, CancellationToken cancellationToken)
        {
            var result = await categoriesService.DeleteCategoryAsync(id, cancellationToken);
            if (!result)
            {
                var category = await categoriesService.GetCategoryByIdAsync(id,cancellationToken);
                ModelState.AddModelError(string.Empty,"The category could not be deleted.");
                return View("DeleteCategory",category);
            }
            TempData.SuccessMessage(
                NotificationExtensions.ActionType.Delete,
                NotificationExtensions.EntityType.Category);
            return RedirectToAction(nameof(Categories));
        }

    }
}
