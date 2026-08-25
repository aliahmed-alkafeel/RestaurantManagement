using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Areas.Dashboard.IServices;
using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.Models;
using System.Security.Claims;

namespace RestaurantManagement.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Route("[area]/[controller]")]
    [Authorize]
    public class GroupsController(IGroupsService groupsService) : Controller
    {
        [Authorize(Roles = nameof(UserRole.AccessEmployees))]
        [HttpGet("")]
        public async Task<IActionResult> Groups()
        {
            var groupsVm = await groupsService.GetAllGroupsAsync();
            return View(groupsVm);
        }

        [Authorize(Roles = nameof(UserRole.ManageEmployees))]
        [HttpGet("EditGroup/{id:guid}")]
        public async Task<IActionResult> EditGroup(Guid id)
        {
            var groupVm = await groupsService.GetGroupByIdAsync(id);
            return View(groupVm);
        }

        [Authorize(Roles = nameof(UserRole.ManageEmployees))]
        [HttpPost("EditGroup/{id:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGroup(GroupViewModel model)
        {
            var result = await groupsService.UpdateGroupAsync(model, Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if (!result)
            {
                ModelState.AddModelError("", "This update is not allowed");
                return View(model);
            }
            return RedirectToAction(nameof(Groups));
        }
    }
}
