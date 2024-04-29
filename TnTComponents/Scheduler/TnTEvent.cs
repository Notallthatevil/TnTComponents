namespace TnTComponents.Scheduler;

using TnTComponents;

public record TnTEvent {
    private static int _id = 0;
    public int Id { get; internal set; } = Interlocked.Increment(ref _id);
    public required string Title { get; init; }
    public required DateTimeOffset EventStart { get; init; }
    public required DateTimeOffset EventEnd { get; init; }

    public TimeOnly StartTime => TimeOnly.FromTimeSpan(EventStart.TimeOfDay);
    public TimeOnly EndTime => TimeOnly.FromTimeSpan(EventEnd.TimeOfDay);
    public DateOnly StartDate => DateOnly.FromDateTime(EventStart.Date);
    public DateOnly EndDate => DateOnly.FromDateTime(EventEnd.Date);

    public TnTColor BackgroundColor { get; set; } = TnTColor.Tertiary;
    public TnTColor ForegroundColor { get; set; } = TnTColor.OnTertiary;


    public bool Overlaps(TnTEvent other) {
        return EventStart < other.EventEnd && other.EventStart < EventEnd;
    }
}