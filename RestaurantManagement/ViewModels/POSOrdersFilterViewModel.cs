using RestaurantManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.ViewModels
{
    public class POSOrdersFilterViewModel : BaseQuery
    {
        public string? Search { get; set; }

        public OrderStatus? Status { get; set; }

        public string Sort { get; set; } = "oldest";

    }

}

