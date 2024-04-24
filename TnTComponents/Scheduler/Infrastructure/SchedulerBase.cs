using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Scheduler.Infrastructure;
public abstract class SchedulerBase : ComponentBase {

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    public virtual string? CssClass { get; }

    public virtual string? CssStyle { get; }

    [Parameter]
    public ICollection<TnTEvent> Events { get; set; } = [];

    [Parameter]
    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTimeOffset.Now.Date);
    [Parameter]
    public TimeOnly MinimumTime { get; set; } = new(0, 0);
    [Parameter]
    public TimeOnly MaximumTime { get; set; } = new(23, 59);
    [Parameter]
    public IEnumerable<DisabledDateRange> DisabledDateRanges { get; set; } = [];
    [Parameter]
    public IEnumerable<DisabledTimeRange> DisabledTimeRanges { get; set; } = [];
    public ElementReference Element { get; protected set; }

    public abstract void AddView(ScheduleViewBase view);

    public abstract void RemoveView(ScheduleViewBase view);

    public virtual bool AddEvent(TnTEvent @event) {
        Events.Add(@event);
        return true;
    }

    public virtual void RemoveEvent(TnTEvent @event) {
        Events.Remove(@event);
    }

    public abstract bool IsSelectedView(ScheduleViewBase scheduleViewBase);
}

