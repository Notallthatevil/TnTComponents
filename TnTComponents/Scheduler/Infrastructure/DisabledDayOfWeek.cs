using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Scheduler.Infrastructure;
internal class DisabledDayOfWeek(TnTDayOfWeekFlag dayOfWeekFlag, TimeOnly disabledStartTime, TimeOnly disabledEndTime) : TnTDisabledDateTime {
    public override bool IsDateTimeDisabled(DateTimeOffset dateTimeOffset) =>
        dayOfWeekFlag.HasDay(dateTimeOffset.DayOfWeek) && TimeOnly.FromTimeSpan(dateTimeOffset.TimeOfDay) >= disabledStartTime && TimeOnly.FromTimeSpan(dateTimeOffset.TimeOfDay) < disabledEndTime;

    public override bool IsDayDisabled(DateOnly date) => dayOfWeekFlag.HasDay(date.DayOfWeek);

    public override bool IsTimeSlotDisabled(DayOfWeek dayOfWeek, TimeOnly timeSlot) => dayOfWeekFlag.HasDay(dayOfWeek) && timeSlot >= disabledStartTime && timeSlot < disabledEndTime;
}

