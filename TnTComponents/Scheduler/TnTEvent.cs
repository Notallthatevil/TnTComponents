using Microsoft.AspNetCore.Components;
using TnTComponents.Core;

namespace TnTComponents.Scheduler;

public class TnTEvent {
    public DateTimeOffset EndTime { get; set; } = DateTimeOffset.MaxValue;
    public DateTimeOffset StartTime { get; set; } = DateTimeOffset.MinValue;
    public string Title { get; set; } = string.Empty;


    internal bool Overlaps(TnTEvent other) {
        return StartTime < other.EndTime && EndTime > other.StartTime;
    }
}