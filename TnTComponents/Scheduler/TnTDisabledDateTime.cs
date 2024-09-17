using TnTComponents.Scheduler.Infrastructure;

namespace TnTComponents.Scheduler;

public abstract class TnTDisabledDateTime {

    public static TnTDisabledDateTime DisabledDate(DateOnly date) => new DisabledDateOnly(date);

    public static TnTDisabledDateTime DisabledDateRange(DateTimeOffset disabledStart, DateTimeOffset disabledEnd) => new DisabledDateRange(disabledStart, disabledEnd);

    public static TnTDisabledDateTime DisabledDayOfWeek(TnTDayOfWeekFlag dayOfWeekFlag, TimeOnly disabledStartTime, TimeOnly disabledEndTime) => new DisabledDayOfWeek(dayOfWeekFlag, disabledStartTime, disabledEndTime);

    internal abstract bool IsDateTimeDisabled(DateTimeOffset dateTimeOffset);

    internal abstract bool IsDayDisabled(DateOnly date);

    internal abstract bool IsTimeSlotDisabled(DayOfWeek dayOfWeek, TimeOnly timeSlot);
}