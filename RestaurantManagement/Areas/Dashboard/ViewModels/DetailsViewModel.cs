using RestaurantManagement.ViewModels;

namespace RestaurantManagement.Areas.Dashboard.ViewModels
{
    public class DetailsViewModel : BaseDto
    {            
        public decimal TotalSales { get; set; }
        public int TotalOrders { get; set; }
        public decimal TodaySales { get; set; }
        public int TodayOrders { get; set; }
        public List<DailySales> SalesLast7Days { get; set; } = [];
        public List<TopSellingItem> TopSellingItems { get; set; } = [];
        public List<CategorySales> SalesByCategory { get; set; } = [];
        public List<TopSellingItem> MostPopularToday { get; set; } = [];
    }
    public class DailySales
    {
        public DateTime Date { get; set; }

        public decimal Sales { get; set; }
    }


    public class TopSellingItem
    {
        public Guid ItemId { get; set; }

        public string ItemName { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal TotalSales { get; set; }
    }


    public class CategorySales
    {
        public Guid CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public decimal TotalSales { get; set; }
    }
}