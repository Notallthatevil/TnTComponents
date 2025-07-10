using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using TnTComponents.Core;
using TnTComponents.Ext;
using TnTComponents.Interfaces;
using TnTComponents.Scheduler.Events;

namespace TnTComponents.Scheduler.Infrastructure;

/// <summary>
///     Represents the base class for schedule views in the scheduler component.
/// </summary>
/// <typeparam name="TEventType">The type of the event.</typeparam>
public abstract class ScheduleViewBase<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TEventType> : TnTComponentBase, IDisposable where TEventType : TnTEvent {

    /// <summary>
    ///     Gets the event that is currently being dragged.
    /// </summary>
    protected TEventType? DraggingEvent { get; private set; }

    /// <summary>
    ///     Gets the scheduler instance.
    /// </summary>
    [CascadingParameter]
    protected TnTScheduler<TEventType> Scheduler { get; private set; } = default!;

    /// <summary>
    ///     Decrements the specified date.
    /// </summary>
    /// <param name="src">The source date.</param>
    /// <returns>The decremented date.</returns>
    public abstract DateOnly DecrementDate(DateOnly src);

    /// <summary>
    ///     Disposes the schedule view.
    /// </summary>
    public void Dispose() {
        Scheduler.RemoveScheduleView(this);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Gets the first visible date.
    /// </summary>
    /// <returns>The first visible date.</returns>
    public abstract DateOnly GetFirstVisibleDate();

    /// <summary>
    ///     Gets the last visible date.
    /// </summary>
    /// <returns>The last visible date.</returns>
    public abstract DateOnly GetLastVisibleDate();

    /// <summary>
    ///     Increments the specified date.
    /// </summary>
    /// <param name="src">The source date.</param>
    /// <returns>The incremented date.</returns>
    public abstract DateOnly IncrementDate(DateOnly src);

    /// <summary>
    ///     Refreshes the schedule view.
    /// </summary>
    public abstract void Refresh();

    /// <summary>
    ///     Rounds up the specified date and time to the nearest interval.
    /// </summary>
    /// <param name="dateTime">The date and time.</param>
    /// <param name="interval">The interval.</param>
    /// <returns>The rounded up date and time.</returns>
    protected static DateTimeOffset Ceiling(DateTimeOffset dateTime, TimeSpan interval) {
        var overflow = dateTime.Ticks % interval.Ticks;
        return overflow == 0 ? dateTime : dateTime.AddTicks(interval.Ticks - overflow);
    }

    /// <summary>
    ///     Rounds down the specified date and time to the nearest interval.
    /// </summary>
    /// <param name="dateTime">The date and time.</param>
    /// <param name="interval">The interval.</param>
    /// <returns>The rounded down date and time.</returns>
    protected static DateTimeOffset Floor(DateTimeOffset dateTime, TimeSpan interval) => dateTime.AddTicks(-(dateTime.Ticks % interval.Ticks));

    /// <summary>
    ///     Rounds the specified date and time to the nearest interval.
    /// </summary>
    /// <param name="dateTime">The date and time.</param>
    /// <param name="interval">The interval.</param>
    /// <returns>The rounded date and time.</returns>
    protected static DateTimeOffset Round(DateTimeOffset dateTime, TimeSpan interval) {
        var halfIntervalTicks = (interval.Ticks + 1) >> 1;
        return dateTime.AddTicks(halfIntervalTicks - ((dateTime.Ticks + halfIntervalTicks) % interval.Ticks));
    }

    /// <summary>
    ///     Calculates the date and time offset based on the pointer Y offset and date.
    /// </summary>
    /// <param name="pointerYOffset">The pointer Y offset.</param>
    /// <param name="date">          The date.</param>
    /// <returns>The calculated date and time offset.</returns>
    protected abstract DateTimeOffset CalculateDateTimeOffset(double pointerYOffset, DateOnly date);

    /// <summary>
    ///     Handles the event click asynchronously.
    /// </summary>
    /// <param name="event">The event.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected Task EventClickedAsync(TEventType? @event) => Scheduler.EventClickedCallback.InvokeAsync(@event);

    /// <summary>
    ///     Handles the event slot click asynchronously.
    /// </summary>
    /// <param name="slot">The slot.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected Task EventSlotClickedAsync(DateTimeOffset slot) => Scheduler.EventSlotClickedCallback.InvokeAsync(slot);

    /// <summary>
    ///     Handles the drag end event asynchronously.
    /// </summary>
    /// <param name="args">The drag event arguments.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected virtual Task OnDragEndAsync(DragEventArgs args) {
        if (Scheduler.AllowDraggingEvents) {
            DraggingEvent = null;
        }
        return Task.CompletedTask;
    }

    /// <summary>
    ///     Handles the drag start event asynchronously.
    /// </summary>
    /// <param name="args"> The drag event arguments.</param>
    /// <param name="event">The event.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected virtual Task OnDragStartAsync(DragEventArgs args, TEventType @event) {
        if (Scheduler.AllowDraggingEvents) {
            DraggingEvent = @event;
        }
        return Task.CompletedTask;
    }

    /// <summary>
    ///     Handles the drop event asynchronously.
    /// </summary>
    /// <param name="args">        The drag event arguments.</param>
    /// <param name="newStartTime">The new start time.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected virtual Task OnDropAsync(DragEventArgs args, DateTimeOffset newStartTime) {
        if (DraggingEvent is not null && Scheduler.AllowDraggingEvents) {
            var duration = DraggingEvent.Duration;
            DraggingEvent.EventStart = newStartTime;
            DraggingEvent.EventEnd = newStartTime.Add(duration);
            var e = Scheduler.Events.FirstOrDefault(e => e.Id == DraggingEvent.Id);
            Refresh();
        }
        return Task.CompletedTask;
    }

    /// <summary>
    ///     Initializes the schedule view.
    /// </summary>
    protected override void OnInitialized() {
        base.OnInitialized();

        if (Scheduler is null) {
            throw new InvalidOperationException($"A {GetType().Name} must be used within a {nameof(TnTScheduler<TEventType>)} component.");
        }

        Scheduler.AddScheduleView(this);
    }
}