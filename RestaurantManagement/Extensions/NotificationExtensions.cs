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
                ActionType.Create =>
                    $"The {entity} created successfully.",

                ActionType.Update =>
                    $"The {entity} updated successfully.",

                ActionType.Delete =>
                    $"The {entity} deleted successfully.",

                ActionType.Terminate =>
                    $"The {entity} terminated successfully.",

                _ =>
                    "Operation completed successfully."
            };

            tempData["SuccessMessage"] = message;
        }

        public static void WarningMessage(
            this ITempDataDictionary tempData,
            ActionType action,
            EntityType entity)
        {
            string message = action switch
            {
                ActionType.CantDelete =>
                    $"The {entity} can't be deleted.",

                ActionType.CantTerminate =>
                    $"The {entity} can't be terminated.",

                _ =>
                    "Operation could not be completed."
            };

            tempData["WarningMessage"] = message;
        }

        public enum ActionType
        {
            Create,
            Update,
            Delete,
            Terminate,
            CantDelete,
            CantTerminate
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