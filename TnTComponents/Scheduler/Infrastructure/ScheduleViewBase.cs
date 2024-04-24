using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Scheduler.Infrastructure;
public abstract class ScheduleViewBase : ComponentBase, IDisposable {
    [CascadingParameter]
    public SchedulerBase Scheduler { get; set; } = default!;
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }



    public virtual string? CssClass { get; }
    public virtual string? CssStyle { get; }
    public ElementReference Element { get; protected set; }


    public void Dispose() {
        GC.SuppressFinalize(this);
        Scheduler.RemoveView(this);
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

    protected bool IsSlotDisabled(DateOnly day, TimeOnly time) {
        var slot = new DateTimeOffset(day, time, TimeZoneInfo.Local.BaseUtcOffset);
        return Scheduler.DisabledTimeRanges.Any(other => time >= other.StartTime && time < other.EndTime && other.DaysOfWeek.HasDay(slot.DayOfWeek)) ||
           Scheduler.DisabledDateRanges.Any(other => slot >= other.Start && slot < other.End);
    }

    protected Task CreateEventAsync(DateOnly day, TimeOnly time) {
        return Task.CompletedTask;
    }

    protected abstract IEnumerable<DateOnly> GetVisibleDates();
    protected abstract IEnumerable<TimeOnly> GetTimeSlots();
}

