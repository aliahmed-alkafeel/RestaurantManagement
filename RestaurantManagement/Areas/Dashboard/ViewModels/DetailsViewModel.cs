namespace RestaurantManagement.Areas.Dashboard.ViewModels
{
    public class DetailsViewModel
    {            
        public decimal TotalSales { get; set; }
        public int TotalOrders { get; set; }
        public decimal TodaySales { get; set; }
        public int TodayOrders { get; set; }
        public List<DailySalesViewModel> SalesLast7Days { get; set; } = [];
        public List<TopSellingItemViewModel> TopSellingItems { get; set; } = [];
        public List<CategorySalesViewModel> SalesByCategory { get; set; } = [];
        public List<TopSellingItemViewModel> MostPopularToday { get; set; } = [];
    }
    public class DailySalesViewModel
    {
        public DateTime Date { get; set; }

        public decimal Sales { get; set; }
    }


    public class TopSellingItemViewModel
    {
        public Guid ItemId { get; set; }

        public string ItemName { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal TotalSales { get; set; }
    }


    public class CategorySalesViewModel
    {
        public Guid CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public decimal TotalSales { get; set; }
    }
}