namespace RestaurantManagement.Extensions
{
    public static class DateTimeExtensions
    {
        private static readonly TimeZoneInfo IraqTimeZone =
            TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");


        // Iraq Local Time → UTC
        public static DateTime ToUtc(this DateTime dateTime)
        {
            var unspecified = DateTime.SpecifyKind(
                dateTime,
                DateTimeKind.Unspecified);

            return TimeZoneInfo.ConvertTimeToUtc(
                unspecified,
                IraqTimeZone);
        }


        // UTC → Iraq Local Time
        public static DateTime ToIraqTime(this DateTime dateTime)
        {
            var utc = DateTime.SpecifyKind(
                dateTime,
                DateTimeKind.Utc);

            return TimeZoneInfo.ConvertTimeFromUtc(
                utc,
                IraqTimeZone);
        }
    }


}
