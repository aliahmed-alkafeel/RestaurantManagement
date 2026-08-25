using Microsoft.AspNetCore.Mvc;

namespace RestaurantManagement.Areas.Dashboard.Controllers
{
    public class OrdersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
