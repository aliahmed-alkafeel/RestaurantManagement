using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Areas.Dashboard.IServices;
using RestaurantManagement.Models;

namespace RestaurantManagement.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Route("[area]/[controller]")]
    [Authorize(Roles = nameof(UserRole.AccessPayments))]
    public class DetailsController(IDetailsService DetailsService) : Controller
    {
        [Authorize(Roles = nameof(UserRole.AccessDetails))]
        public async Task<IActionResult> Details()
        {
            var model = await DetailsService.GetDetailsAsync();

            return View(model);
        }
    }
}
