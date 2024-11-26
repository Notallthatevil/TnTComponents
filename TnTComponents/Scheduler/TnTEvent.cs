namespace TnTComponents.Scheduler;

using TnTComponents;

public record TnTEvent {
    private static int _id = 0;
    public int Id { get; internal set; } = Interlocked.Increment(ref _id);
    public string Title { get; init; } = default!;
    public string? Description { get; set; }
    public DateTimeOffset EventStart { get; set; }
    public DateTimeOffset EventEnd { get; set; }
    public virtual TimeOnly StartTime => TimeOnly.FromTimeSpan(EventStart.LocalDateTime.TimeOfDay);
    public virtual TimeOnly EndTime => TimeOnly.FromTimeSpan(EventEnd.LocalDateTime.TimeOfDay);
    public virtual DateOnly StartDate => DateOnly.FromDateTime(EventStart.LocalDateTime.Date);
    public virtual DateOnly EndDate => DateOnly.FromDateTime(EventEnd.LocalDateTime.Date);
    public virtual TimeSpan Duration => EventEnd - EventStart;

    public TnTColor BackgroundColor { get; set; } = TnTColor.Tertiary;
    public TnTColor ForegroundColor { get; set; } = TnTColor.OnTertiary;
    public TnTColor TintColor { get; set; } = TnTColor.SurfaceTint;
    public TnTColor OnTintColor { get; set; } = TnTColor.OnTertiary;

    public bool Overlaps(TnTEvent other) {
        return EventStart < other.EventEnd && other.EventStart < EventEnd;
    }
}