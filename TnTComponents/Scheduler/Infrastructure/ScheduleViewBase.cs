using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using TnTComponents.Core;
using TnTComponents.Ext;
using TnTComponents.Interfaces;
using TnTComponents.Scheduler.Events;

namespace TnTComponents.Scheduler.Infrastructure;

public abstract class ScheduleViewBase<TEventType> : TnTComponentBase, IDisposable where TEventType : TnTEvent {
    protected TEventType? DraggingEvent { get; private set; }

    [CascadingParameter]
    protected TnTScheduler<TEventType> Scheduler { get; private set; } = default!;

    [Inject]
    private IJSRuntime _jsRuntime { get; set; } = default!;

    public abstract DateOnly DecrementDate(DateOnly src);

    public void Dispose() {
        Scheduler.RemoveScheduleView(this);
    }

    public abstract DateOnly IncrementDate(DateOnly src);

    public abstract void Refresh();

    protected static DateTimeOffset Ceiling(DateTimeOffset dateTime, TimeSpan interval) {
        var overflow = dateTime.Ticks % interval.Ticks;

        return overflow == 0 ? dateTime : dateTime.AddTicks(interval.Ticks - overflow);
    }

    protected static DateTimeOffset Floor(DateTimeOffset dateTime, TimeSpan interval) {
        return dateTime.AddTicks(-(dateTime.Ticks % interval.Ticks));
    }

    protected static DateTimeOffset Round(DateTimeOffset dateTime, TimeSpan interval) {
        var halfIntervalTicks = (interval.Ticks + 1) >> 1;

        return dateTime.AddTicks(halfIntervalTicks - ((dateTime.Ticks + halfIntervalTicks) % interval.Ticks));
    }

    protected abstract DateTimeOffset CalculateDateTimeOffset(double pointerYOffset, DateOnly date);

    protected Task EventClickedAsync(TEventType? @event) => Scheduler.EventClickedCallback.InvokeAsync(@event);

    protected Task EventSlotClickedAsync(DateTimeOffset slot) => Scheduler.EventSlotClickedCallback.InvokeAsync(slot);

    public abstract DateOnly GetFirstVisibleDate();
    public abstract DateOnly GetLastVisibleDate();
    protected virtual Task OnDragStartAsync(DragEventArgs args, TEventType @event) {
        DraggingEvent = @event;
        return Task.CompletedTask;
    }

    protected virtual Task OnDragEndAsync(DragEventArgs args) {
        DraggingEvent = null;
        return Task.CompletedTask;
    }

    protected virtual Task OnDropAsync(DragEventArgs args, DateTimeOffset newStartTime) {
        if (DraggingEvent is not null) {
            var duration = DraggingEvent.Duration;
            DraggingEvent.EventStart = newStartTime;
            DraggingEvent.EventEnd = newStartTime.Add(duration);
            var e = Scheduler.Events.FirstOrDefault(e => e.Id == DraggingEvent.Id);
            Refresh();
        }
        return Task.CompletedTask;
    }

    protected override void OnInitialized() {
        base.OnInitialized();

        if (Scheduler is null) {
            throw new InvalidOperationException($"A {GetType().Name} must be used within a {nameof(TnTScheduler<TEventType>)} component.");
        }

        Scheduler.AddScheduleView(this);
    }
}