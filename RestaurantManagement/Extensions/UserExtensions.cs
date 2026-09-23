using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Models;
using System.Security.Claims;
using System.Security.Principal;

namespace RestaurantManagement.Extensions
{
    public static class UserExtensions
    {
        public static Guid? GetUserId(this IPrincipal principal)
        {
            if (principal?.Identity is ClaimsIdentity claimsIdentity)
            {
                // Look for the NameIdentifier claim (standard for user IDs)
                var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    return userId;
                }
            }

            return null;
        }
    }




    
}
