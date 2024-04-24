using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Scheduler;
public readonly struct DisabledTime {
    public TnTDayOfWeekFlag DayOfWeek { get; init; }
    public readonly TimeOnly StartTime { get; init; }
    public readonly TimeOnly EndTime { get; init; }
}

