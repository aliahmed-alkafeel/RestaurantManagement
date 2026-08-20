using RestaurantManagement.Areas.Dashboard.ViewModels;

namespace RestaurantManagement.Areas.Dashboard.IServices
{
    public interface IGroupsService
    {
        Task<List<GroupViewModel>> GetAllGroupsAsync();
        Task<GroupViewModel> GetGroupByIdAsync(Guid Id);
        Task<bool> UpdateGroupAsync(GroupViewModel model, Guid ModifierId);
    }
}
