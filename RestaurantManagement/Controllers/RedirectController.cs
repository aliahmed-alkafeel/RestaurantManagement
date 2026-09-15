using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Models;

namespace RestaurantManagement.Controllers
{
    public class RedirectController : Controller
    {
        public IActionResult Index()
        {

            if (User.IsInRole(nameof(UserRole.AccessDetails)))
            {
                return RedirectToAction("Details", "Details",new
                {
                    area = "Dashboard"
                });
            }

            if (User.IsInRole(nameof(UserRole.AccessEmployees)))
            {
                return RedirectToAction("Employees", "Employees", new
                {
                    area = "Dashboard"
                });
            }

            if (User.IsInRole(nameof(UserRole.AccessOrders)))
            {
                return RedirectToAction("Orders", "Orders", new
                {
                    area = "Dashboard"
                });
            }

            if (User.IsInRole(nameof(UserRole.AccessDiscounts)))
            {
                return RedirectToAction("Discounts", "Discounts", new
                {
                    area = "Dashboard"
                });
            }
            if (User.IsInRole(nameof(UserRole.AccessCategories)))
            {
                return RedirectToAction("Categories", "Categories", new
                {
                    area = "Dashboard"
                });
            }
            if (User.IsInRole(nameof(UserRole.AccessItems)))
            {
                return RedirectToAction("Items", "Items", new
                {
                    area = "Dashboard"
                });
            }

  
            if (User.IsInRole(nameof(UserRole.AccessGroups)))
            {
                return RedirectToAction("Groups", "Groups", new
                {
                    area = "Dashboard"
                });
            }


            return RedirectToAction("AccessDenied", "Auth", new
            {
                area = ""
            });
        }

        public IActionResult DashToHome()
        {
            if (User.IsInRole(nameof(UserRole.ManageOrders)))
            {
                return RedirectToAction("NewOrder", "POS", new
                {
                    area = "Dashboard"
                });
            }

            return RedirectToAction("Index", "Redirect", new
            {
                area = ""
            });
        }
    }
}
