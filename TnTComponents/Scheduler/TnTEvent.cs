namespace TnTComponents.Scheduler;

using TnTComponents;

public record TnTEvent {
    private static int _id = 0;
    public int Id { get; internal set; } = Interlocked.Increment(ref _id);
    public required string Title { get; init; }
    public string? Description { get; set; }
    public required DateTimeOffset EventStart { get; init; }
    public required DateTimeOffset EventEnd { get; init; }

    public TimeOnly StartTime => TimeOnly.FromTimeSpan(EventStart.LocalDateTime.TimeOfDay);
    public TimeOnly EndTime => TimeOnly.FromTimeSpan(EventEnd.LocalDateTime.TimeOfDay);
    public DateOnly StartDate => DateOnly.FromDateTime(EventStart.LocalDateTime.Date);
    public DateOnly EndDate => DateOnly.FromDateTime(EventEnd.LocalDateTime.Date);
    public TimeSpan Duration => EventEnd - EventStart;

    public TnTColor BackgroundColor { get; set; } = TnTColor.Tertiary;
    public TnTColor ForegroundColor { get; set; } = TnTColor.OnTertiary;

    public bool Overlaps(TnTEvent other) {
        return EventStart < other.EventEnd && other.EventStart < EventEnd;
    }
}