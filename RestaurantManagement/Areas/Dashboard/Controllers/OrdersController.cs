using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class OrdersController(IOrdersService ordersService) : Controller
    {
        [Authorize(Roles = nameof(UserRole.AccessOrders))]
        [HttpGet("")]
        public async Task<IActionResult> Orders(CancellationToken cancellationToken)
        {
            var Orders = await ordersService.GetAllOrdersAsync(cancellationToken);
            return View(Orders);
        }
        //[Authorize(Roles = nameof(UserRole.AccessOrders))]
        //[HttpGet("OrderDetails/{id:guid}")]
        //public async Task<IActionResult> OrderDetails(Guid id, CancellationToken cancellationToken)
        //{
        //    var Orders = await ordersService.GetOrderByIdAsync(id, cancellationToken);
        //    return View(Orders);
        //}

        //[Authorize(Roles = nameof(UserRole.ManageOrders))]
        //[HttpGet("CreateOrder")]
        //public async Task<IActionResult> CreateOrder()
        //{
        //    return View();
        //}
        [Authorize(Roles = nameof(UserRole.ManageOrders))]
        [HttpGet("EditOrder/{id:guid}")]
        public async Task<IActionResult> EditOrder(Guid id, CancellationToken cancellationToken)
        {
            var order = await ordersService.GetOrderByIdAsync(id, cancellationToken);
            return View(order);
        }

        [Authorize(Roles = nameof(UserRole.ManageOrders))]
        [HttpPost("EditPostOrder/{id:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPostOrder(OrderViewModel model)
        {
            ModelState.Remove("Order.OrderName");
            if (!ModelState.IsValid)
            {
                return View("EditOrder",model);
            }
            var result = await ordersService.UpdateOrderAsync(model);
            if (!result)
            {
                ModelState.AddModelError("", "This update is not allowed");
                return View("EditOrder",model);
            }
            TempData.SuccessMessage(
                NotificationExtensions.ActionType.Update,
                NotificationExtensions.EntityType.Order);
            return RedirectToAction(nameof(Orders));
        }


        [Authorize(Roles = nameof(UserRole.ManageOrders))]
        [HttpGet("DeleteOrder/{id:guid}")]
        public async Task<IActionResult> DeleteOrder(Guid id, CancellationToken cancellationToken)
        {
            var emps = await ordersService.GetOrderByIdAsync(id, cancellationToken);
            return View(emps);
        }
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UserRole.ManageOrders))]
        [HttpPost("ConfirmedDeleteOrder/{id:guid}")]
        public async Task<IActionResult> ConfirmedDeleteOrder(Guid id, CancellationToken cancellationToken)
        {
            await ordersService.DeleteOrderAsync(id, cancellationToken);
            TempData.SuccessMessage(
                NotificationExtensions.ActionType.Delete,
                NotificationExtensions.EntityType.Order);
            return RedirectToAction(nameof(Orders));
        }
    }

}

