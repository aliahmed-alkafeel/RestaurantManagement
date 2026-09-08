using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Areas.Dashboard.IServices;
using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.Data;
using RestaurantManagement.IRepositories;

namespace RestaurantManagement.Areas.Details.Services
{
    public class DetailsService(IUnitOfWork unitOfWork) : IDetailsService
    {
        public async Task<DetailsViewModel> GetDetailsAsync()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            var startDate = today.AddDays(-6);
            var totalSales = await unitOfWork.ItemOrders
                .NoTrackingSelect()
                .SumAsync(io => io.Price * io.Quantity);

            var totalOrders = await unitOfWork.Orders
                .NoTrackingSelect()
                .CountAsync();

            var todaySales = await unitOfWork.ItemOrders
                .NoTrackingSelect()
                .Where(io =>
                    io.Order.OrderDate >= today &&
                    io.Order.OrderDate < tomorrow).SumAsync(io => io.Price * io.Quantity);

            var todayOrders = await unitOfWork.Orders
                .NoTrackingSelect()
                .Where(o =>
                    o.OrderDate >= today &&
                    o.OrderDate < tomorrow).CountAsync();

            var salesLast7Days = await unitOfWork.ItemOrders
                .NoTrackingSelect()
                .Where(io =>
                    io.Order.OrderDate >= startDate)
                .GroupBy(io => io.Order.OrderDate.Date)
                .Select(g => new DailySalesViewModel()
                {
                    Date = g.Key,
                    Sales = g.Sum(io => io.Price * io.Quantity)
                })
                .ToListAsync();
            var completeSalesLast7Days = Enumerable
    .Range(0, 7)
    .Select(i =>
    {
        var date = startDate.AddDays(i);

        var existing = salesLast7Days
            .FirstOrDefault(x => x.Date == date);

        return new DailySalesViewModel
        {
            Date = date,
            Sales = existing?.Sales ?? 0
        };
    })
    .ToList();
            var topSellingItems = await unitOfWork.ItemOrders
                .NoTrackingSelect()
                .GroupBy(io => new
                {
                    io.ItemId,
                    io.Item.ItemName
                })
                .Select(g => new TopSellingItemViewModel
                {
                    ItemId = g.Key.ItemId,

                    ItemName = g.Key.ItemName,

                    Quantity = g.Sum(x => x.Quantity),

                    TotalSales = g.Sum(x =>
                        x.Price * x.Quantity)
                })
                .OrderByDescending(x => x.Quantity)
                .Take(5)
                .ToListAsync();

                 var salesByCategory = await unitOfWork.ItemOrders
                .NoTrackingSelect()
                .GroupBy(io => new
                {
                    io.Item.CategoryId,
                    io.Item.Category.CategoryName
                })
                .Select(g => new CategorySalesViewModel
                {
                    CategoryId = g.Key.CategoryId,
                    CategoryName = g.Key.CategoryName,
                    TotalSales = g.Sum(x =>
                        x.Price * x.Quantity)
                })
                .OrderByDescending(x => x.TotalSales)
                .ToListAsync();

                 var mostPopularToday = await unitOfWork.ItemOrders
                .NoTrackingSelect()
                .Where(io =>
                 io.Order.OrderDate >= today &&
                 io.Order.OrderDate < tomorrow)
                .GroupBy(io => new
                {
                    io.ItemId,
                    io.Item.ItemName
                })
                .Select(g => new TopSellingItemViewModel
                {
                    ItemId = g.Key.ItemId,
                    ItemName = g.Key.ItemName,
                    Quantity = g.Sum(x => x.Quantity),
                    TotalSales = g.Sum(x =>
                    x.Price * x.Quantity)
                })
                .OrderByDescending(x => x.Quantity)
                .Take(5)
                .ToListAsync();

                return new DetailsViewModel
                {
                TotalSales = totalSales,

                TotalOrders = totalOrders,

                TodaySales = todaySales,
                TodayOrders = todayOrders,

                SalesLast7Days = completeSalesLast7Days,

                TopSellingItems = topSellingItems,

                SalesByCategory = salesByCategory,

                MostPopularToday = mostPopularToday
            };
        }
    }
}
