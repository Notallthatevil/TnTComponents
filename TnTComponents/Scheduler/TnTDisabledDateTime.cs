using TnTComponents.Scheduler.Infrastructure;

namespace TnTComponents.Scheduler;

public abstract class TnTDisabledDateTime {

    public static TnTDisabledDateTime DisabledDate(DateOnly date) {
        return new DisabledDateOnly(date);
    }

    public static TnTDisabledDateTime DisabledDateRange(DateTimeOffset disabledStart, DateTimeOffset disabledEnd) {
        return new DisabledDateRange(disabledStart, disabledEnd);
    }

    public static TnTDisabledDateTime DisabledDayOfWeek(TnTDayOfWeekFlag dayOfWeekFlag, TimeOnly disabledStartTime, TimeOnly disabledEndTime) {
        return new DisabledDayOfWeek(dayOfWeekFlag, disabledStartTime, disabledEndTime);
    }

    public abstract bool IsDateTimeDisabled(DateTimeOffset dateTimeOffset);

    public abstract bool IsDayDisabled(DateOnly date);

    public bool IsDisabledDateTime(DateTimeOffset dateTimeOffset) {
        return IsDateTimeDisabled(dateTimeOffset);
    }

    public bool IsDisabledDay(DateOnly date) {
        return IsDayDisabled(date);
    }

    public bool IsDisabledTimeSlot(DayOfWeek dayOfWeek, TimeOnly timeSlot) {
        return IsTimeSlotDisabled(dayOfWeek, timeSlot);
    }

    public abstract bool IsTimeSlotDisabled(DayOfWeek dayOfWeek, TimeOnly timeSlot);
}