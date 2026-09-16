using RestaurantManagement.Models;

namespace RestaurantManagement.ViewModels
{
    public class ItemAvailabilityViewModel
    {
        public List<ItemAvailabilityTypeViewModel> AvailableGroups { get; set; } = [];

        public List<ItemAvailabilityTypeViewModel> UnavailableGroups { get; set; } = [];
    }

    public class ItemAvailabilityTypeViewModel
    {
        public CategoryType Type { get; set; }

        public string TypeName => Type.ToString();

        public List<ItemAvailabilityCategoryViewModel> Categories { get; set; } = [];
    }

    public class ItemAvailabilityCategoryViewModel
    {
        public Guid CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public List<AvailabilityItemViewModel> Items { get; set; } = [];
    }

    public class AvailabilityItemViewModel
    {
        public Guid Id { get; set; }

        public string ItemName { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public bool IsAvailable { get; set; }

        public Guid CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public CategoryType Type { get; set; }

        public string TypeName => Type.ToString();

        public string ImageUrl { get; set; } = string.Empty;
    }
}
