using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Areas.Dashboard.IServices;
using RestaurantManagement.Areas.Dashboard.Services;
using RestaurantManagement.Models;
using RestaurantManagement.ViewModels;
using System.Diagnostics;

namespace RestaurantManagement.Controllers
{
    [Authorize]
    public class HomeController(IItemsService itemsService) : Controller
    {
        public async Task<IActionResult> IndexAsync()
        {
            var Items = await itemsService.GetAllItemsAsync();
            return View(Items);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
