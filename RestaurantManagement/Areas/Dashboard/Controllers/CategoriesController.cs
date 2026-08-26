using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RestaurantManagement.Areas.Dashboard.IServices;
using RestaurantManagement.Areas.Dashboard.Services;
using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.Models;
using System.Security.Claims;

namespace RestaurantManagement.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Route("[area]/[controller]")]
    public class CategoriesController(ICategoriesService categoriesService) : Controller
    {
        [Authorize(Roles = nameof(UserRole.AccessCategories))]
        [HttpGet("")]
        public async Task<IActionResult> Categories()
        {
            var categories = await categoriesService.GetAllCategoriesAsync();
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
        public async Task<IActionResult> CreateCategory(CategoryViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await categoriesService.CreateCategoryAsync(model);
            if (result is false)
            {
                ModelState.AddModelError("", "The Category is Regestered");
                return View(model);
            }
            return RedirectToAction(nameof(Categories));
        }
        [Authorize(Roles = nameof(UserRole.ManageCategories))]
        [HttpGet("EditCategory/{id:guid}")]
        public async Task<IActionResult> EditCategory(Guid id)
        {
            var order = await categoriesService.GetCategoryByIdAsync(id);
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
            var result = await categoriesService.UpdateCategoryAsync(model, Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if (!result)
            {
                ModelState.AddModelError("", "This update is not allowed");
                return View(model);
            }
            return RedirectToAction(nameof(Categories));
        }

        [Authorize(Roles = nameof(UserRole.ManageCategories))]
        [HttpGet("DeleteCategory/{id:guid}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var emps = await categoriesService.GetCategoryByIdAsync(id);
            return View(emps);
        }
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UserRole.ManageCategories))]
        [HttpPost("ConfirmedDeleteCategory/{id:guid}")]
        public async Task<IActionResult> ConfirmedDeleteCategory(Guid id)
        {
            await categoriesService.DeleteCategoryAsync(id, Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            return RedirectToAction(nameof(Categories));
        }

    }
}
