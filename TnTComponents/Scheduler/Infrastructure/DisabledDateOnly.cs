using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Scheduler.Infrastructure;
internal class DisabledDateOnly(DateOnly _date) : TnTDisabledDateTime {
    public override bool IsDateTimeDisabled(DateTimeOffset dateTimeOffset) => DateOnly.FromDateTime(dateTimeOffset.Date) == _date;

    public override bool IsDayDisabled(DateOnly date) => date == _date;

    public override bool IsTimeSlotDisabled(DayOfWeek dayOfWeek, TimeOnly timeSlot) => _date.DayOfWeek == dayOfWeek;
}

