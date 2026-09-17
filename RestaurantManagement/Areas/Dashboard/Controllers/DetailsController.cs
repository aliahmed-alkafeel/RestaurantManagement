using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Areas.Dashboard.IServices;
using RestaurantManagement.Models;

namespace RestaurantManagement.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Route("[area]/[controller]")]
    [Authorize(Roles = nameof(UserRole.AccessDetails))]
    public class DetailsController(IDetailsService DetailsService) : Controller
    {
        [Authorize(Roles = nameof(UserRole.AccessDetails))]
        public async Task<IActionResult> Details(CancellationToken cancellationToken)
        {
            var model = await DetailsService.GetDetailsAsync(cancellationToken);

            return View(model);
        }
    }
}
