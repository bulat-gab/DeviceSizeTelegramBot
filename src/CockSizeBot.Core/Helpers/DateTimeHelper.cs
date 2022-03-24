namespace CockSizeBot.Core.Helpers;

public static class DateTimeHelper
{
    public static TimeSpan GetNextResetTime()
    {
        var now = DateTime.UtcNow;
        var tomorrow = now.Date.AddDays(1);

        TimeSpan nextResetTimestamp = tomorrow - now;

        return nextResetTimestamp;
    }
}
