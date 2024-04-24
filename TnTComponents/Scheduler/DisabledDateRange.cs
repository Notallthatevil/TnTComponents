using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Scheduler;
public readonly struct DisabledDateRange {
    public required DateTimeOffset Start { get; init; }
    public required DateTimeOffset End { get; init; }
}

