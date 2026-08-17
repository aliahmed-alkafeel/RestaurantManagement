using Microsoft.AspNetCore.Mvc;

namespace RestaurantManagement.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Employees()
        {
            
            return View();
        }
    }
}
