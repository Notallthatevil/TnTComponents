using Microsoft.AspNetCore.Components;
using TnTComponents.Core;
using TnTComponents.Ext;
using TnTComponents.Scheduler.Events;

namespace TnTComponents.Scheduler.Infrastructure;

public abstract class ScheduleViewBase<TEventType> : ComponentBase, ITnTComponentBase, IDisposable where TEventType : TnTEvent {

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter] public bool? AutoFocus { get; set; }

    public virtual string? CssClass => null;

    public virtual string? CssStyle => null;

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public EventCallback<TEventType> DragStartCallback { get; set; }

    [Parameter]
    public EventCallback<TnTDropEventArgs<TEventType>> DropEventCallback { get; set; }

    public ElementReference Element { get; protected set; }

    [Parameter]
    public EventCallback<TEventType> EventClickedCallback { get; set; }

    public string? Id { get; private set; }

    [Parameter]
    public TimeSpan Interval { get; set; } = TimeSpan.FromMinutes(30);

    [Parameter]
    public EventCallback<DateTimeOffset> TimeSlotClickedCallback { get; set; }

    [CascadingParameter]
    protected TnTScheduler<TEventType> Scheduler { get; set; } = default!;

    protected TEventType? DraggingEvent;

    public void Dispose() {
        GC.SuppressFinalize(this);
        Scheduler.RemoveScheduleView(this);
    }

    internal abstract GridPosition? GetEventPosition(TnTEvent task);

    protected virtual void DragEventEnd() {
        DraggingEvent = null;
    }

    protected IEnumerable<TEventType> GetEventsOnDate(DateOnly date) {
        var startDateTime = new DateTime(date, TimeOnly.MinValue);
        var endDateTime = new DateTime(date, TimeOnly.MaxValue);
        var events = Scheduler.Events.Where(t => t.EventStart < endDateTime && startDateTime < t.EventEnd);

        foreach (var @event in events) {
            if (@event.EventStart.Date == @event.EventEnd.Date) {
                yield return @event with {
                    EventStart = @event.EventStart.Round(Interval),
                    EventEnd = @event.EventEnd.Round(Interval)
                };
            }
            else {
                yield return @event with {
                    EventStart = (startDateTime > @event.EventStart ? startDateTime : @event.EventStart).Round(Interval),
                    EventEnd = (endDateTime < @event.EventEnd ? endDateTime : @event.EventEnd).Round(Interval)
                };
            }
        }
    }

    protected abstract IEnumerable<TnTEvent> GetTasksForDates(DateOnly startDate, DateOnly endDate);

    protected IEnumerable<TnTEvent> GetTasksForSlot(DateTime dateTime) {
        return Scheduler.Events.Where(t => t.EventStart <= dateTime && t.EventEnd > dateTime);
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

    protected abstract IEnumerable<DateOnly> GetVisibleDates();

    protected bool IsDisabledSlot(DayOfWeek dayOfWeek, TimeOnly timeSlot) {
        return Scheduler.DisabledDateTimes.Any(disabledDateTime => disabledDateTime.IsDisabledTimeSlot(dayOfWeek, timeSlot));
    }

    protected virtual async Task OnDropEvent(DateTimeOffset dateTimeOffset) {
        if (Scheduler.DisableDragAndDrop || DraggingEvent is null) {
            return;
        }

        await DropEventCallback.InvokeAsync(new TnTDropEventArgs<TEventType>(DraggingEvent, dateTimeOffset));

        DraggingEvent = null;
        StateHasChanged();
    }

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

    protected virtual async Task TimeSlotClicked(DateTimeOffset dateTimeOffset) {
        await TimeSlotClickedCallback.InvokeAsync(dateTimeOffset);
    }

    public abstract DateOnly? IncrementPage(DateOnly date);
    public abstract DateOnly? DecrementPage(DateOnly date);

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