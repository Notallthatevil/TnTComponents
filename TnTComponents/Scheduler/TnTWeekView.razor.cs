using Microsoft.AspNetCore.Components;
using System.Text;
using TnTComponents.Core;
using TnTComponents.Scheduler;

namespace TnTComponents;

public partial class TnTWeekView<TEventType> where TEventType : TnTEvent {

    public override string? CssClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-week-view")
        .Build();

    public override string? CssStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddStyle(GetGridTemplateColumns(), string.Empty)
        .Build();

    [Parameter]
    public DayOfWeek StartViewOn { get; set; } = DayOfWeek.Sunday;

    private ICollection<KeyValuePair<TEventType, GridPosition>> _events = [];
    private SortedDictionary<TimeOnly, GridPosition> _timeSlots = [];
    private SortedDictionary<DateOnly, GridPosition> _visibleDates = [];

    public override DateOnly? DecrementPage(DateOnly date) {
        return date.AddDays(-7);
    }

    public override IEnumerable<DateOnly> GetVisibleDates() {
        int diff;
        if (StartViewOn > Scheduler.DisplayedDate.DayOfWeek) {
            diff = 7 - (int)StartViewOn + (int)Scheduler.DisplayedDate.DayOfWeek;
        }
        else {
            diff = Math.Abs((int)StartViewOn - (int)Scheduler.DisplayedDate.DayOfWeek);
        }

        if (diff >= 7) {
            diff %= 7;
        }

        var startDate = Scheduler.DisplayedDate.AddDays(-diff);

        for (int i = 0; i < 7; i++) {
            yield return startDate.AddDays(i);
        }
    }

    public override DateOnly? IncrementPage(DateOnly date) {
        return date.AddDays(7);
    }

    public override void Refresh() {
        _timeSlots = new SortedDictionary<TimeOnly, GridPosition>(GetTimeSlots().Select((time, index) => new { time, index }).ToDictionary(x => x.time, x => new GridPosition() {
            ColumnIndex = 1,
            RowIndex = x.index + 2,
            RowSpan = 1,
            ColumnSpan = 1
        }));

        var columnIndex = 2;
        _visibleDates.Clear();
        _events.Clear();
        foreach (var date in GetVisibleDates()) {
            var events = GetEventsOnDate(date);

            if (events.Count() <= 1) {
                _visibleDates.Add(date, new GridPosition() {
                    ColumnIndex = columnIndex,
                    RowIndex = 1,
                    RowSpan = 1,
                    ColumnSpan = 1
                });
                columnIndex++;

                if (events.Any()) {
                    var eventPosition = GetEventPosition(events.First());
                    if (eventPosition.HasValue) {
                        _events.Add(new KeyValuePair<TEventType, GridPosition>(events.First(), eventPosition.Value));
                    }
                }
            }
            else {
                var overlaps = TnTWeekView<TEventType>.CountOverlaps(events);
                _visibleDates.Add(date, new GridPosition() {
                    ColumnIndex = columnIndex,
                    RowIndex = 1,
                    RowSpan = 1,
                    ColumnSpan = overlaps + 1
                });

                foreach (var pair in events.Select((@event, index) => new { Event = @event, Index = index })) {
                    var @event = pair.Event;
                    var index = pair.Index;
                    var eventPosition = GetEventPosition(@event);
                    if (eventPosition.HasValue) {
                        _events.Add(new KeyValuePair<TEventType, GridPosition>(@event, eventPosition.Value with {
                            ColumnIndex = columnIndex + index
                        }));
                    }
                }
                columnIndex += overlaps + 1;
            }
        }
        base.Refresh();
    }

    internal override GridPosition? GetEventPosition(TnTEvent task) {
        if (_timeSlots.TryGetValue(task.StartTime, out var startTime) &&
            _visibleDates.TryGetValue(task.StartDate, out var startDate)) {
            var rowIndex = startTime.RowIndex;
            var columnIndex = startDate.ColumnIndex;

            if (task.StartDate == task.EndDate &&
                _timeSlots.TryGetValue(task.EndTime, out var endTime)) {
                return new GridPosition {
                    ColumnIndex = columnIndex,
                    RowIndex = rowIndex,
                    RowSpan = endTime.RowIndex - rowIndex
                };
            }
            else if (_timeSlots.Count != 0) {
                return new GridPosition {
                    ColumnIndex = columnIndex,
                    RowIndex = rowIndex,
                    RowSpan = _timeSlots.Last().Value.RowIndex - rowIndex + 1 // Plus one to account for last row in column.
                };
            }
        }

        return null;
    }

    protected override IEnumerable<TnTEvent> GetTasksForDates(DateOnly startDate, DateOnly endDate) {
        var startDateTime = new DateTime(startDate, TimeOnly.MinValue);
        var endDateTime = new DateTime(endDate, TimeOnly.MaxValue);

        var tasks = Scheduler.Events.Where(t => t.EventStart < endDateTime && startDateTime < t.EventEnd);
        foreach (var @event in tasks) {
            if (@event.EventStart.Date == @event.EventEnd.Date) {
                yield return @event;
            }
            else {
                var eventStart = @event.EventStart;
                var eventEnd = new DateTimeOffset(DateOnly.FromDateTime(eventStart.AddDays(1).Date), TimeOnly.MinValue, eventStart.Offset);

                var subEvent = @event with {
                    EventEnd = eventEnd
                };

                yield return subEvent;

                while (subEvent.EventEnd != @event.EventEnd) {
                    var newEventStart = new DateTimeOffset(DateOnly.FromDateTime(subEvent.EventStart.AddDays(1).Date), TimeOnly.MinValue, eventStart.Offset);
                    var newEventEnd = new DateTimeOffset(DateOnly.FromDateTime(subEvent.EventEnd.AddDays(1).Date), TimeOnly.MaxValue, eventStart.Offset);
                    newEventEnd = newEventEnd < @event.EventEnd ? newEventEnd : @event.EventEnd;

                    if (newEventStart == newEventEnd) {
                        break;
                    }

                    subEvent = @event with {
                        EventStart = newEventStart,
                        EventEnd = newEventEnd
                    };

                    yield return subEvent;
                }
            }
        }
    }

    protected override void OnParametersSet() {
        base.OnParametersSet();
        Refresh();
    }

    private static int CountOverlaps(IEnumerable<TnTEvent> events) {
        // TODO Fix this so that overlaps are only counted with overlapping events
        if (events.Any()) {
            return events.Count() - 1;
            var overlaps = 0;
            var currentEvent = events.First();
            foreach (var @event in events.Skip(1)) {
                if (@event.EventStart < currentEvent.EventEnd) {
                    overlaps++;
                }
                else {
                    currentEvent = @event;
                }
            }
            return overlaps;
        }
        return 0;
    }

    private async Task DragEventStart(TEventType @event) {
        DraggingEvent = @event;
        await DragStartCallback.InvokeAsync(@event);
    }

    private async Task EventClicked(TEventType @event) {
        await EventClickedCallback.InvokeAsync(@event);
    }

    private string? GetGridTemplateColumns() {
        var stringBuilder = new StringBuilder("grid-template-columns: 5rem");
        foreach (var date in _visibleDates) {
            const int BaseWidth = 13;
            for (var i = 0; i < date.Value.ColumnSpan; i++) {
                stringBuilder.Append(' ').Append($"{BaseWidth / date.Value.ColumnSpan}%");
            }
        }
        return stringBuilder.Append(';').ToString();
    }
}