using RestaurantManagement.ViewModels;

namespace RestaurantManagement.IServices
{
    public interface IItemAvailabilityService
    {
        Task<ItemAvailabilityViewModel> GetAvailabilityAsync(CancellationToken cancellationToken = default);
        Task<AvailabilityItemViewModel?> ToggleAvailabilityAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
