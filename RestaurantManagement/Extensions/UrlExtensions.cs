using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Models;

namespace RestaurantManagement.Extensions
{
    public static class UrlExtensions
    {
        public static string? HomeToDash(
            this IUrlHelper url,
            System.Security.Claims.ClaimsPrincipal user)
        {
            if (user.IsInRole(nameof(UserRole.AccessDetails)))
                return url.Action("Details", "Details", new { area = "Dashboard" });

            if (user.IsInRole(nameof(UserRole.AccessEmployees)))
                return url.Action("Employees", "Employees", new { area = "Dashboard" });

            if (user.IsInRole(nameof(UserRole.AccessOrders)))
                return url.Action("Orders", "Orders", new { area = "Dashboard" });

            if (user.IsInRole(nameof(UserRole.AccessDiscounts)))
                return url.Action("Discounts", "Discounts", new { area = "Dashboard" });

            if (user.IsInRole(nameof(UserRole.AccessCategories)))
                return url.Action("Categories", "Categories", new { area = "Dashboard" });

            if (user.IsInRole(nameof(UserRole.AccessItems)))
                return url.Action("Items", "Items", new { area = "Dashboard" });

            if (user.IsInRole(nameof(UserRole.AccessGroups)))
                return url.Action("Groups", "Groups", new { area = "Dashboard" });

            return url.Action("AccessDenied", "Auth", new { area = "" });
        }

        public static string? DashToHome(
            this IUrlHelper url,
            System.Security.Claims.ClaimsPrincipal user)
        {
            if (user.IsInRole(nameof(UserRole.ManageOrders)))
                return url.Action("NewOrder", "POS", new { area = "" });

            if (user.IsInRole(nameof(UserRole.ManageItems)))
                return url.Action("ItemsAvailability", "POS", new { area = "" });
            return HomeToDash(url, user);
        }
    }
}
