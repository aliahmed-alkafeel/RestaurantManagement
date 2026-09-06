using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Areas.Dashboard.IServices;
using RestaurantManagement.Areas.Dashboard.Services;
using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.Models;
using RestaurantManagement.ViewModels;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace RestaurantManagement.Controllers
{
    [Authorize]
    public class POSController(IItemsService itemsService,IOrdersService ordersService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> NewOrder()
        {
            var items = await itemsService.GetAllItemsAsync();
            return View(items);
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [Authorize(Roles = nameof(UserRole.ManageOrders))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder(CreateOrderViewModel model)
        {
            ModelState.Remove("Order.OrderName");
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid order data"
                });
            }
            var result = await ordersService.CreateOrderAsync(model);
            if (!result)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "This Order is not allowed"
                });
            }
            return Ok(new
            {
                success = true,
                message = "Order created successfully."
            });
        }
        [Authorize(Roles = nameof(UserRole.ManageOrders))]
        [HttpGet]
        public async Task<IActionResult> POSOrders()
        {
            var orders = await ordersService.GetPOSOrders();
            return View(orders);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
