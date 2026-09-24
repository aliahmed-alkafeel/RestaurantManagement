using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace RestaurantManagement.Extensions
{
    public static class NotificationExtensions
    {
        public static void SuccessMessage(
                this ITempDataDictionary tempData,
                ActionType action,
                EntityType entity)
            {
                string message = action switch
                {
                    ActionType.Create => $"{entity.ToString()} created successfully.",
                    ActionType.Update => $"{entity.ToString()} updated successfully.",
                    ActionType.Delete => $"{entity.ToString()} deleted successfully.",
                    _ => "Operation completed successfully."
                };

                tempData["SuccessMessage"] = message;
            }
        public enum ActionType
        {
            Create,
            Update,
            Delete
        }

        public enum EntityType
        {
            Employee,
            Group,
            Category,
            Item,
            Order,
            Discount
        }
    }
}
