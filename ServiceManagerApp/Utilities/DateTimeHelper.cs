namespace ServiceManagerApp.Utilities
{
    public static class DateTimeHelper
    {
        public static TimeSpan CalculateDuration(int? hours, int? minutes)
        {
            if(hours < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(hours), hours, "Duration time cannot be negative.");
            }
            if (minutes.HasValue)
            {
                ArgumentOutOfRangeException.ThrowIfNegative(minutes.Value);
            }

            int totalMinutes = (hours ?? 0) * 60 + (minutes ?? 0);
            return TimeSpan.FromMinutes(totalMinutes);
        }

        public static int GetMinutesFromTimeSpan(TimeSpan? duration)
        {
            return duration == null ? 0 : (int)duration.Value.TotalMinutes;
        }
    }
}
