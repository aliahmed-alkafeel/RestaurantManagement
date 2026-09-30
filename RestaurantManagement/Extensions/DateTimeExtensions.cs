namespace RestaurantManagement.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ToUtc(this DateTime dateTime)
        {
            return dateTime.Kind switch
            {
                DateTimeKind.Utc => dateTime,
                DateTimeKind.Local => dateTime.ToUniversalTime(),
                _ => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc) // Unspecified: نفترض أنها UTC
            };
        }
    }
}
