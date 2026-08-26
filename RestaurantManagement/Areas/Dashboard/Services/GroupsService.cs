using Microsoft.AspNetCore.Identity;
using RestaurantManagement.Areas.Dashboard.IServices;
using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.IRepositories;
using RestaurantManagement.Models;

namespace RestaurantManagement.Areas.Dashboard.Services
{
    public class GroupsService : IGroupsService
    {
        private readonly IUnitOfWork _unitOfWork;
        public GroupsService(IUnitOfWork unitOfWork, IPasswordHasher<Employee> passwordHasher)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<GroupViewModel>> GetAllGroupsAsync()
        {
            var groups = await _unitOfWork.Groups.GetAllGroupsWithRolesAsync();
            List<GroupViewModel> groupsVm = [];
            foreach (var group in groups)
            {
                groupsVm.Add(new GroupViewModel
                {
                    Id = group.Id,
                    GroupName = group.GroupName,
                    GroupRoles = group.GroupRoles
                });
            }
            return (groupsVm);
        }

        public async Task<GroupViewModel> GetGroupByIdAsync(Guid id)
        {
            var group = await _unitOfWork.Groups.GetGroupWithRolesByIdAsync(id);
            GroupViewModel groupVm = new GroupViewModel
            {
                Id = group.Id,
                GroupName = group.GroupName,
                GroupRoles = group.GroupRoles
            };
            return (groupVm);
        }

        public async Task<bool> UpdateGroupAsync(GroupViewModel model, Guid ModifierId)
        {
            if (model is null) throw new ArgumentNullException();
            var group = await _unitOfWork.Groups.GetByIdAsync(model.Id);
            if (group is null) throw new KeyNotFoundException("There is no such group");
            var roles = await _unitOfWork.Roles.GetRolesByNamesAsync(model.Roles);
            await _unitOfWork.GroupsRoles.DeleteByGroupIdAsync(model.Id);
            foreach(var role in roles)
            {
                group.GroupRoles.Add(new GroupRole
                {
                    GroupId = group.Id,
                    RoleId = role.Id
                });
            }
            _unitOfWork.Groups.Update(group, ModifierId);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
