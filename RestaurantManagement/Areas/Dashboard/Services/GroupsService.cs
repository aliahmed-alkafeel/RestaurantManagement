using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Areas.Dashboard.IServices;
using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.IRepositories;
using RestaurantManagement.Models;
using RestaurantManagement.Repositories;

namespace RestaurantManagement.Areas.Dashboard.Services
{
    public class GroupsService(IUnitOfWork unitOfWork) : IGroupsService
    {
        public async Task<bool> CreateGroupAsync(GroupViewModel model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));
            var hasRepeatedName = await unitOfWork.Groups.Select()
                .Where(g => g.GroupName == model.GroupName).AnyAsync(model.CancellationToken);
            if (hasRepeatedName) return false;
            var group = new Group
            {
                Id = Guid.NewGuid(),
                GroupName = model.GroupName
            };
            var roles = await unitOfWork.Roles.Select()
                .Where(r => model.Roles.Contains(r.RoleName)).ToListAsync(model.CancellationToken);
            foreach (var role in roles)
            {
                group.GroupRoles.Add(new GroupRole
                {
                    GroupId = group.Id,
                    RoleId = role.Id
                });
            }
            await unitOfWork.Groups.AddAsync(group, model.CancellationToken);
            await unitOfWork.SaveChangesAsync(model.CancellationToken);
            return true;
        }

        public async Task<bool> DeleteGroupAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var group = await unitOfWork.Groups.GetByIdAsync(id, cancellationToken);
            if (group is null) throw new InvalidOperationException("There is no such group");
            if (group.GroupName == InitUserGroup.Administrator.ToString()) return false;
            unitOfWork.Groups.Delete(group);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<List<GroupViewModel>> GetAllGroupsAsync(CancellationToken cancellationToken = default)
        {
            return await unitOfWork.Groups
                .NoTrackingSelect().Include(g => g.GroupRoles).ThenInclude(gr => gr.Role)
                .Select(group =>
                 new GroupViewModel
                {
                    Id = group.Id,
                    GroupName = group.GroupName,
                    GroupRoles = group.GroupRoles
                }).ToListAsync(cancellationToken);
            }

        public async Task<GroupViewModel> GetGroupByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var group = await unitOfWork.Groups.NoTrackingSelect()
                .Include(g => g.GroupRoles)
                .ThenInclude(gr => gr.Role).FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
            if (group is null) throw new ArgumentNullException(nameof(group));
            return new GroupViewModel
            {
                Id = group.Id,
                GroupName = group.GroupName,
                GroupRoles = group.GroupRoles
            };
          
        }

        public async Task<bool> UpdateGroupAsync(GroupViewModel model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));
            var group = await unitOfWork.Groups.GetByIdAsync(model.Id, model.CancellationToken);
            if (group is null) throw new InvalidOperationException("There is no such group");
            if (model.GroupName == InitUserGroup.Administrator.ToString() ||
                group.GroupName == InitUserGroup.Administrator.ToString()) 
                return false;
            group.GroupName = model.GroupName;
            var roles = await unitOfWork.Roles.Select()
                .Where(r => model.Roles.Contains(r.RoleName)).ToListAsync(model.CancellationToken);
            var oldGroupRoles = unitOfWork.GroupsRoles.Select()
                .Where(g => g.GroupId == model.Id);
            unitOfWork.GroupsRoles.DeleteRange(oldGroupRoles);
            foreach (var role in roles)
            {
                group.GroupRoles.Add(new GroupRole
                {
                    GroupId = group.Id,
                    RoleId = role.Id
                });
            }
            unitOfWork.Groups.Update(group);
            await unitOfWork.SaveChangesAsync(model.CancellationToken);
            return true;
        }
    }
}
