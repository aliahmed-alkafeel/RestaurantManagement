using Azure.Core.Pipeline;

namespace RestaurantManagement.ViewModels
{
    public class BaseQuery
    {
        public CancellationToken CancellationToken { get; set; }
    }
}
