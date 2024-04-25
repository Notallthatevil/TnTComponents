using Microsoft.AspNetCore.Components;
using TnTComponents.Core;

namespace TnTComponents.Scheduler.Infrastructure;

public abstract class ScheduleViewBase : ComponentBase, IDisposable {

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    public virtual string? CssClass { get; }

    public virtual string? CssStyle { get; }

    public ElementReference Element { get; protected set; }

    [CascadingParameter]
    public SchedulerBase Scheduler { get; set; } = default!;

    public void Dispose() {
        GC.SuppressFinalize(this);
        Scheduler.RemoveView(this);
    }

    protected Task CreateEventAsync(DateOnly day, TimeOnly time) {
        Scheduler.AddEvent(new TnTEvent {
            StartTime = new DateTimeOffset(day, time, TimeZoneInfo.Local.BaseUtcOffset),
            EndTime = new DateTimeOffset(day, time, TimeZoneInfo.Local.BaseUtcOffset).AddHours(2),
            Title = $"Event{Scheduler.Events.Count + 1}"
        });
        return Task.CompletedTask;
    }

    protected abstract IEnumerable<TimeOnly> GetTimeSlots();

    protected abstract IEnumerable<DateOnly> GetVisibleDates();

    protected bool IsSlotDisabled(DateOnly day, TimeOnly time) {
        var slot = new DateTimeOffset(day, time, TimeZoneInfo.Local.BaseUtcOffset);
        return Scheduler.DisabledTimeRanges.Any(other => time >= other.StartTime && time < other.EndTime && other.DaysOfWeek.HasDay(slot.DayOfWeek)) ||
           Scheduler.DisabledDateRanges.Any(other => slot >= other.Start && slot < other.End);
    }

    protected override void OnInitialized() {
        base.OnInitialized();
        Scheduler.AddView(this);
    }

    protected override void OnParametersSet() {
        base.OnParametersSet();
        if (Scheduler is null) {
            throw new InvalidOperationException($"The {GetType().Name} component requires a parent {nameof(SchedulerBase)} component.");
        }
    }

    internal abstract Task<BoundingClientRect> CalculateEventBoundingRect(TnTEvent @event);
}