using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Scheduler;
public class TnTEvent {
    public DateTimeOffset StartTime { get; set; } = DateTimeOffset.MinValue;
    public DateTimeOffset EndTime { get; set; } = DateTimeOffset.MaxValue;
    public string Title { get; set; } = string.Empty;
}

