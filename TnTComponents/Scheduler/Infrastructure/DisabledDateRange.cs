using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Scheduler.Infrastructure;
internal class DisabledDateRange(DateTimeOffset disabledStart, DateTimeOffset disabledEnd) : TnTDisabledDateTime {
    public override bool IsDateTimeDisabled(DateTimeOffset dateTimeOffset) {
        return dateTimeOffset >= disabledStart && dateTimeOffset <= disabledEnd;
    }

    public override bool IsDayDisabled(DateOnly date) {
        return date >= DateOnly.FromDateTime(disabledStart.Date) && date <= DateOnly.FromDateTime(disabledEnd.Date);
    }

    public override bool IsTimeSlotDisabled(DayOfWeek dayOfWeek, TimeOnly timeSlot) {
        if (dayOfWeek == disabledStart.DayOfWeek && dayOfWeek == disabledEnd.DayOfWeek) {
            return timeSlot >= TimeOnly.FromTimeSpan(disabledStart.TimeOfDay) && timeSlot <= TimeOnly.FromTimeSpan(disabledEnd.TimeOfDay);
        }
        return false;
    }
}

