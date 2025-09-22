using System.Diagnostics.CodeAnalysis;

namespace TnTComponents.Scheduler.Events;

/// <summary>
///     Provides data for the drop event in the scheduler.
/// </summary>
/// <typeparam name="TEventType">The type of the event.</typeparam>
/// <param name="event">                The event that was dropped.</param>
/// <param name="droppedDateTimeOffset">The date and time when the event was dropped.</param>
[ExcludeFromCodeCoverage]
public class TnTDropEventArgs<TEventType>(TEventType @event, DateTimeOffset droppedDateTimeOffset) where TEventType : TnTEvent {

    /// <summary>
    ///     Gets the date and time when the event was dropped.
    /// </summary>
    public DateTimeOffset DroppedDateTimeOffset => droppedDateTimeOffset;

    /// <summary>
    ///     Gets the event that was dropped.
    /// </summary>
    public TEventType Event => @event;
}