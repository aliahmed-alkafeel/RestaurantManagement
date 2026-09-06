using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Areas.Dashboard.IServices;
using RestaurantManagement.Areas.Dashboard.Services;
using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.Migrations;
using RestaurantManagement.Models;
using System.Security.Claims;

namespace RestaurantManagement.Areas.Dashboard.Controllers
{
        [Area("Dashboard")]
        [Route("[area]/[controller]")]
    public class ItemsController(IItemsService itemsService) : Controller
    {
            //[Authorize(Roles = nameof(UserRole.AccessItems))]
            //[HttpGet("")]
            //public async Task<IActionResult> Items()
            //{
            //    var Items = await itemsService.GetPagedItemsAsync();
            //    return View(Items);
            //}
            [Authorize(Roles = nameof(UserRole.AccessItems))]
            [HttpGet]
            public async Task<IActionResult> Items(ItemFilterViewModel model)
            {
                var Items = await itemsService.GetPagedItemsAsync(model);
                return View(Items);
            }
            

            [Authorize(Roles = nameof(UserRole.ManageItems))]
            [HttpGet("CreateItem")]
            public async Task<IActionResult> CreateItem()
            {
                return View();
            }

            [Authorize(Roles = nameof(UserRole.ManageItems))]
            [HttpPost("CreateItem")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> CreateItem(ItemViewModel model)
            {
            ModelState.Remove("Category.CategoryName");
            if (!ModelState.IsValid) return View(model);
                var result = await itemsService.CreateItemAsync(model);
                if (result is false)
                {
                    ModelState.AddModelError("", "The Item is Regestered");
                    return View(model);
                }
                return RedirectToAction(nameof(Items));
            }
            [Authorize(Roles = nameof(UserRole.ManageItems))]
            [HttpGet("EditItem/{id:guid}")]
            public async Task<IActionResult> EditItem(Guid id)
            {
                var emps = await itemsService.GetItemByIdAsync(id);
                return View(emps);
            }
            [Authorize(Roles = nameof(UserRole.ManageItems))]
            [HttpPost("EditItem/{id:guid}")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> EditItem(ItemViewModel model)
            {
            ModelState.Remove("Category.CategoryName");
            if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var result = await itemsService.UpdateItemAsync(model, Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));
                if (!result)
                {
                    ModelState.AddModelError("", "This update is not allowed");
                    return View(model);
                }
                return RedirectToAction(nameof(Items));
            }

            [Authorize(Roles = nameof(UserRole.ManageItems))]
            [HttpGet("DeleteItem/{id:guid}")]
            public async Task<IActionResult> DeleteItem(Guid id)
            {
                var emps = await itemsService.GetItemByIdAsync(id);
                return View(emps);
            }
            [ValidateAntiForgeryToken]
            [Authorize(Roles = nameof(UserRole.ManageItems))]
            [HttpPost("ConfirmedDeleteItem/{id:guid}")]
            public async Task<IActionResult> ConfirmedDeleteItem(Guid id)
            {
                await itemsService.DeleteItemAsync(id, Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));
                return RedirectToAction(nameof(Items));
            }
        [HttpGet("GetCategoriesByType")]
        [Authorize(Roles = nameof(UserRole.AccessCategories))]
        public async Task<IActionResult> GetCategoriesByType(CategoryType type)
        {
            var categories = await itemsService.GetCategoriesByTypeAsync(type);
            return Json(categories.Select(c => new
            {
                id = c.Id,
                categoryName = c.CategoryName
            }));
        }
        [HttpGet("GetItemsByCategory")]
        [Authorize(Roles = nameof(UserRole.AccessCategories))]
        public async Task<IActionResult> GetItemsByCategory(Guid categoryId)
        {
            var items = await itemsService.GetItemsByCategoryId(categoryId);
            return Json(items);
        }
      }
    }