using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Scheduler;
public readonly struct DisabledTimeRange {
    public required TnTDayOfWeekFlag DaysOfWeek { get; init; }
    public required TimeOnly StartTime { get; init; }
    public required TimeOnly EndTime { get; init; }
}

