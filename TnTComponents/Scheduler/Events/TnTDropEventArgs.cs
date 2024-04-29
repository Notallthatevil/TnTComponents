namespace TnTComponents.Scheduler.Events;

public class TnTDropEventArgs<TEventType>(TEventType @event, DateTimeOffset droppedDateTimeOffset) where TEventType : TnTEvent {
    public DateTimeOffset DroppedDateTimeOffset => droppedDateTimeOffset;
    public TEventType Event => @event;
}