using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Areas.Dashboard.IServices;
using RestaurantManagement.Areas.Dashboard.Services;
using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.Models;
using RestaurantManagement.ViewModels;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using RestaurantManagement.IServices;

namespace RestaurantManagement.Controllers
{
    [Authorize]
    //[Area("")]
    //[Route("[controller]/[action]")]
    public class POSController(IItemsService itemsService,IOrdersService ordersService,
        IItemAvailabilityService itemAvailabilityService) : Controller
    {
        [HttpGet]
        [Authorize(Roles = nameof(UserRole.ManageOrders))]
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
        public async Task<IActionResult> POSOrders(
            POSOrdersFilterViewModel filter,
            CancellationToken cancellationToken)
        {
            var result = await ordersService.GetPOSOrdersAsync(
                filter,
                cancellationToken);

            return View(result);
        }
        [Authorize(Roles = nameof(UserRole.ManageOrders))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrderStatus(
            [FromBody] OrderStatusViewModel? model)
        {
            if (model is null)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Model is null"
                });
            }

            var modifierId = Guid.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var result = await ordersService.UpdateOrderAsync(model, modifierId);

            if (!result)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Order not found",
                    orderId = model.OrderId
                });
            }

            return Ok(new
            {
                success = true
            });
        }

        [HttpGet]
        [Authorize(Roles = nameof(UserRole.ManageItems))]
        public async Task<IActionResult> ItemsAvailability(
            CancellationToken cancellationToken)
        {
            var model =
                await itemAvailabilityService
                    .GetAvailabilityAsync(
                        cancellationToken);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRole.ManageItems))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleAvailability(
            Guid id,
            CancellationToken cancellationToken)
        {
            var item =
                await itemAvailabilityService
                    .ToggleAvailabilityAsync(
                        id,
                        cancellationToken);


            if (item is null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Item not found."
                });
            }


            return Json(new
            {
                success = true,

                id = item.Id,

                isAvailable =
                    item.IsAvailable,

                itemName =
                    item.ItemName,

                price =
                    item.Price,

                categoryId =
                    item.CategoryId,

                categoryName =
                    item.CategoryName,

                type =
                    (int)item.Type,

                typeName =
                    item.TypeName,

                imageUrl =
                    item.ImageUrl
            });
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("Error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
        }
    }
}
