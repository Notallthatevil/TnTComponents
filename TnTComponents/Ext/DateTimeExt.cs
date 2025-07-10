namespace TnTComponents.Ext;

internal static class DateTimeExt {

    public static bool Overlaps(DateTime startA, DateTime EndA, DateTime startB, DateTime endB) => startA < endB && EndA > startB;

    public static DateTime Round(this DateTime dateTime, TimeSpan interval) => new((long)Math.Round(dateTime.Ticks / (double)interval.Ticks) * interval.Ticks);

    public static DateTimeOffset Round(this DateTimeOffset dateTimeOffset, TimeSpan interval) => new((long)Math.Round(dateTimeOffset.Ticks / (double)interval.Ticks) * interval.Ticks, dateTimeOffset.Offset);

    public static TimeOnly RoundToNearestMinuteInterval(this TimeOnly time, TimeSpan interval) => new(time.Hour, (int)(Math.Round(time.Minute / (double)interval.Minutes) * interval.Minutes));

    public static DateTime ToDateTime(this DateOnly date) => new(date, default);
}