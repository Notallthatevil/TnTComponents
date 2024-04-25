using Microsoft.AspNetCore.Components;
using TnTComponents.Ext;

namespace TnTComponents.Scheduler.Infrastructure;

public abstract class SchedulerBase : ComponentBase {

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    public virtual string? CssClass { get; }

    public virtual string? CssStyle { get; }

    [Parameter]
    public IEnumerable<DisabledDateRange> DisabledDateRanges { get; set; } = [];

    [Parameter]
    public IEnumerable<DisabledTimeRange> DisabledTimeRanges { get; set; } = [];

    public ElementReference Element { get; protected set; }

    [Parameter]
    public ICollection<TnTEvent> Events { get; set; } = [];

    [Parameter]
    public TimeOnly MaximumTime { get; set; } = new(23, 59);

    [Parameter]
    public TimeOnly MinimumTime { get; set; } = new(0, 0);

    [Parameter]
    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTimeOffset.Now.Date);

    public virtual bool AddEvent(TnTEvent @event) {
        Events.Add(@event);
        return true;
    }

    public abstract void AddView(ScheduleViewBase view);

    public abstract bool IsSelectedView(ScheduleViewBase scheduleViewBase);

    public virtual void RemoveEvent(TnTEvent @event) {
        Events.Remove(@event);
    }

    public abstract void RemoveView(ScheduleViewBase view);

    public IEnumerable<TnTEvent> GetEventsForDate(DateOnly date) {
        var dateTimeDate = date.ToDateTime();
        return Events.Where(e => e.StartTime.Date == dateTimeDate || e.EndTime.Date == dateTimeDate);
    }
}