using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Areas.Dashboard.IServices;
using RestaurantManagement.Areas.Dashboard.Services;
using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.Models;
using System.Security.Claims;

namespace RestaurantManagement.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Route("[area]/[controller]")]
    public class OrdersController(IOrdersService ordersService) : Controller
    {
        [Authorize(Roles = nameof(UserRole.AccessOrders))]
        [HttpGet("")]
        public async Task<IActionResult> Orders()
        {
            var Orders = await ordersService.GetAllOrdersAsync();
            return View(Orders);
        }
        [Authorize(Roles = nameof(UserRole.AccessOrders))]
        [HttpGet("OrderDetails/{id:guid}")]
        public async Task<IActionResult> OrderDetails(Guid id)
        {
            var Orders = await ordersService.GetOrderByIdAsync(id);
            return View(Orders);
        }

        [Authorize(Roles = nameof(UserRole.ManageOrders))]
        [HttpGet("CreateOrder")]
        public async Task<IActionResult> CreateOrder()
        {
            return View();
        }

        [Authorize(Roles = nameof(UserRole.ManageOrders))]
        [HttpPost("EditOrder/{id:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOrder(OrderViewModel model)
        {
            ModelState.Remove("Order.OrderName");
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await ordersService.UpdateOrderAsync(model, Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if (!result)
            {
                ModelState.AddModelError("", "This update is not allowed");
                return View(model);
            }
            return RedirectToAction(nameof(Orders));
        }

        [Authorize(Roles = nameof(UserRole.ManageOrders))]
        [HttpGet("DeleteOrder/{id:guid}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var emps = await ordersService.GetOrderByIdAsync(id);
            return View(emps);
        }
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UserRole.ManageOrders))]
        [HttpPost("ConfirmedDeleteOrder/{id:guid}")]
        public async Task<IActionResult> ConfirmedDeleteOrder(Guid id)
        {
            await ordersService.DeleteOrderAsync(id, Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            return RedirectToAction(nameof(Orders));
        }
    }

}

