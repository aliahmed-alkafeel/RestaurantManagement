using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Areas.Dashboard.IServices;
using RestaurantManagement.Areas.Dashboard.Services;
using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.Models;
using System.Security.Claims;

namespace RestaurantManagement.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Route("[area]/[controller]")]
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
        [HttpGet("CreateGroup")]
        public async Task<IActionResult> CreateGroup()
        {
            return View("ManageGroup", new GroupViewModel());
        }
        [Authorize(Roles = nameof(UserRole.ManageEmployees))]
        [HttpPost("CreateGroup")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGroup(GroupViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await groupsService.CreateGroupAsync(model);
            if (result is false)
            {
                ModelState.AddModelError("", "The Group is Regestered");
                return View("ManageGroup",model);
            }
            return RedirectToAction(nameof(Groups));
        }
        [Authorize(Roles = nameof(UserRole.ManageEmployees))]
        [HttpGet("DeleteGroup/{id:guid}")]
        public async Task<IActionResult> DeleteGroup(Guid id)
        {
            var emps = await groupsService.GetGroupByIdAsync(id);
            return View(emps);
        }
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UserRole.ManageEmployees))]
        [HttpPost("ConfirmedDeleteGroup/{id:guid}")]
        public async Task<IActionResult> ConfirmedDeleteGroup(Guid id)
        {
            await groupsService.DeleteGroupAsync(id, Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            return RedirectToAction(nameof(Groups));
        }
        [Authorize(Roles = nameof(UserRole.ManageEmployees))]
        [HttpGet("EditGroup/{id:guid}")]
        public async Task<IActionResult> EditGroup(Guid id)
        {
            var groupVm = await groupsService.GetGroupByIdAsync(id);
            return View("ManageGroup",groupVm);
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
                return View("ManageGroup",model);
            }
            return RedirectToAction(nameof(Groups));
        }
    }
}