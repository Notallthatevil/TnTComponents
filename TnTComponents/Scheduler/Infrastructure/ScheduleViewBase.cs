using BlazorCalendar.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Core;

namespace TnTComponents.Scheduler.Infrastructure;
public abstract class ScheduleViewBase<TEventType> : ComponentBase, ITnTComponentBase, IDisposable where TEventType : TnTEvent {
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }
    [Parameter] public bool? AutoFocus { get; set; }

    [Parameter]
    public TimeSpan Interval { get; set; } = TimeSpan.FromMinutes(30);

    public virtual string? CssClass => null;
    [Parameter]
    public bool Disabled { get; set; }

    [CascadingParameter]
    protected TnTScheduler<TEventType> Scheduler { get; set; } = default!;

    public ElementReference Element { get; protected set; }

    public string? Id { get; private set; }

    public virtual string? CssStyle => null;
    [Parameter]
    public string HeaderStyle { get; set; }
    protected override void OnInitialized() {
        base.OnInitialized();
        Id ??= TnTComponentIdentifier.NewId();
        Scheduler?.AddScheduleView(this);
    }

    protected override void OnParametersSet() {
        base.OnParametersSet();
        if (Scheduler is null) {
            throw new InvalidOperationException($"{GetType().Name} requires a parent of type {nameof(TnTScheduler<TEventType>)}.");
        }
    }

    public void Dispose() {
        GC.SuppressFinalize(this);
        Scheduler.RemoveScheduleView(this);
    }

    protected IEnumerable<TimeOnly> GetTimeSlots() {
        var startTime = TimeOnly.MinValue;
        var currentTime = startTime;
        var interval = Interval;

        while (currentTime < TimeOnly.MaxValue) {
            yield return currentTime;
            var newTime = currentTime.Add(interval);
            if (newTime <= currentTime) {
                break;
            }
            currentTime = newTime;
        }
    }

    protected IEnumerable<Tasks> GetTasksForSlot(DateTime dateTime) {
        return Scheduler.TasksList.Where(t => t.DateStart <= dateTime && t.DateEnd > dateTime);
    }


    protected abstract IEnumerable<Tasks> GetTasksForDates(DateOnly startDate, DateOnly endDate);
    internal abstract GridPosition GetEventPosition(Tasks task);

    internal readonly record struct GridPosition() {
        public required int RowIndex { get; init; }
        public required int ColumnIndex { get; init; }
        public int RowSpan { get; init; } = 1;
        public int ColumnSpan { get; init; } = 1;

        public string? ToCssString() {
            return CssStyleBuilder.Create()
                   .AddVariable("row-index", RowIndex.ToString())
                   .AddVariable("column-index", ColumnIndex.ToString())
                   .AddVariable("row-span", RowSpan.ToString())
                   .AddVariable("column-span", ColumnSpan.ToString())
                   .Build();
        }
    }
}

