namespace TnTComponents.Scheduler;

public class TnTEvent {
    public DateTimeOffset EndTime { get; set; } = DateTimeOffset.MaxValue;
    public DateTimeOffset StartTime { get; set; } = DateTimeOffset.MinValue;
    public string Title { get; set; } = string.Empty;
}