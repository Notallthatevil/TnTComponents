using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Scheduler.Infrastructure;
internal class DisabledDateOnly(DateOnly _date) : TnTDisabledDateTime {
    internal override bool IsDateTimeDisabled(DateTimeOffset dateTimeOffset) => DateOnly.FromDateTime(dateTimeOffset.Date) == _date;

    internal override bool IsDayDisabled(DateOnly date) => date == _date;

    internal override bool IsTimeSlotDisabled(DayOfWeek dayOfWeek, TimeOnly timeSlot) => _date.DayOfWeek == dayOfWeek;
}

