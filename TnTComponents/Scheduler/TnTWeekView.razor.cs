
using BlazorCalendar;
using BlazorCalendar.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Collections.Immutable;
using System.Text;
using TnTComponents.Core;
using TnTComponents.Ext;
using TnTComponents.Scheduler;

namespace TnTComponents;
partial class TnTWeekView<TEventType> where TEventType : TnTEvent {
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


    private DateTime _firstdate;
    [CascadingParameter(Name = "FirstDate")]
    public DateTime FirstDate {
        get {
            if (_firstdate == DateTime.MinValue) _firstdate = DateTime.Today;
            return _firstdate.Date;
        }
        set {
            _firstdate = value;
        }
    }

    [Parameter]
    public PriorityLabel PriorityDisplay { get; set; } = PriorityLabel.Code;

    [Parameter]
    public bool HighlightToday { get; set; } = false;

    [Parameter]
    public EventCallback<int> OutsideCurrentMonthClick { get; set; }

    [Parameter]
    public EventCallback<ClickEmptyDayParameter> DayClick { get; set; }

    [Parameter]
    public EventCallback<ClickEmptyDayParameter> EmptyDayClick { get; set; }

    [Parameter]
    public EventCallback<ClickTaskParameter> TaskClick { get; set; }

    [Parameter]
    public EventCallback<DragDropParameter> DragStart { get; set; }

    [Parameter]
    public EventCallback<DragDropParameter> DropTask { get; set; }

    private SortedDictionary<DateOnly, GridPosition> _visibleDates = [];
    private SortedDictionary<TimeOnly, GridPosition> _timeSlots = [];
    private ICollection<KeyValuePair<Tasks, GridPosition>> _events = [];

    protected override void OnParametersSet() {
        base.OnParametersSet();

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
                    _events.Add(new KeyValuePair<Tasks, GridPosition>(events.First(), GetEventPosition(events.First())));
                }
            }
            else {
                var overlaps = CountOverlaps(events);
                _visibleDates.Add(date, new GridPosition() {
                    ColumnIndex = columnIndex,
                    RowIndex = 1,
                    RowSpan = 1,
                    ColumnSpan = overlaps + 1
                });

                foreach (var pair in events.Select((@event, index) => new { Event = @event, Index = index })) {
                    var @event = pair.Event;
                    var index = pair.Index;
                    var position = GetEventPosition(@event) with {
                        ColumnIndex = columnIndex + index
                    };
                    _events.Add(new KeyValuePair<Tasks, GridPosition>(@event, position));
                }
                columnIndex += overlaps + 1;
            }
        }
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

    private IEnumerable<Tasks> GetEventsOnDate(DateOnly date) {
        var startDateTime = new DateTime(date, TimeOnly.MinValue);
        var endDateTime = new DateTime(date, TimeOnly.MaxValue);
        var events = Scheduler.TasksList.Where(t => t.DateStart < endDateTime && startDateTime < t.DateEnd);
        var list = new List<Tasks>();

        foreach (var @event in events) {
            if (@event.DateStart.Date == @event.DateEnd.Date) {
                yield return @event with {
                    DateStart = @event.DateStart.Round(Interval),
                    DateEnd = @event.DateEnd.Round(Interval)
                };
            }
            else {
                yield return @event with {
                    DateStart = (startDateTime > @event.DateStart ? startDateTime : @event.DateStart).Round(Interval),
                    DateEnd = (endDateTime < @event.DateEnd ? endDateTime : @event.DateEnd).Round(Interval)
                };
            }
        }
    }

    private Tasks? TaskDragged;
    private async Task HandleDragStart(int taskID) {
        TaskDragged = new Tasks() {
            ID = taskID
        };

        DragDropParameter dragDropParameter = new() {
            taskID = TaskDragged.ID
        };

        await DragStart.InvokeAsync(dragDropParameter);
    }

    private async Task HandleDayOnDrop(DateTime day) {
        if (Scheduler.DisableDragAndDrop || TaskDragged is null) {
            return;
        }

        DragDropParameter dragDropParameter = new() {
            Day = day,
            taskID = TaskDragged.ID
        };

        await DropTask.InvokeAsync(dragDropParameter);

        TaskDragged = null;
        StateHasChanged();
    }

    private string GetBackground(DateTime day) {
        string WeekDaysColor = "#FFF";

        string SaturdayColor = "#ECF4FD";

        string SundayColor = "#DBE7F8";
        int d = (int)day.DayOfWeek;

        if (d == 6) {
            return $"background:{SaturdayColor}";
        }
        else if (d == 0) {
            return $"background:{SundayColor}";
        }

        return $"background:{WeekDaysColor}";
    }

    private async Task ClickDayInternal(MouseEventArgs e, DateTime day) {
        if (!DayClick.HasDelegate)
            return;

        ClickEmptyDayParameter clickEmptyDayParameter = new() {
            Day = day,
            X = e.ClientX,
            Y = e.ClientY
        };

        await DayClick.InvokeAsync(clickEmptyDayParameter);
    }

    private async Task ClickTaskInternal(MouseEventArgs e, int taskID, DateTime day) {
        if (!TaskClick.HasDelegate)
            return;

        List<int> listID = new()
        {
            taskID
        };

        ClickTaskParameter clickTaskParameter = new() {
            IDList = listID,
            X = e.ClientX,
            Y = e.ClientY,
            Day = day
        };

        await TaskClick.InvokeAsync(clickTaskParameter);
    }

    private IEnumerable<DateOnly> GetVisibleDates() {
        var diff = Math.Abs(StartViewOn - Scheduler.FirstDate.DayOfWeek);

        var startDate = DateOnly.FromDateTime(Scheduler.FirstDate.AddDays(-diff));

        for (int i = 0; i < 7; i++) {
            yield return startDate.AddDays(i);
        }
    }

    protected override IEnumerable<Tasks> GetTasksForDates(DateOnly startDate, DateOnly endDate) {
        var startDateTime = new DateTime(startDate, TimeOnly.MinValue);
        var endDateTime = new DateTime(endDate, TimeOnly.MaxValue);
        var tasks = Scheduler.TasksList.Where(t => t.DateStart < endDateTime && startDateTime < t.DateEnd);
        foreach (var @event in tasks) {
            if (@event.DateStart.Date == @event.DateEnd.Date) {
                yield return @event;
            }
            else {
                var eventStart = @event.DateStart;
                var eventEnd = new DateTime(DateOnly.FromDateTime(eventStart.AddDays(1)), TimeOnly.MinValue);

                var subEvent = @event with {
                    DateEnd = eventEnd
                };

                yield return subEvent;

                while (subEvent.DateEnd != @event.DateEnd) {

                    var newEventStart = new DateTime(DateOnly.FromDateTime(subEvent.DateStart.AddDays(1)), TimeOnly.MinValue);
                    var newEventEnd = new DateTime(DateOnly.FromDateTime(subEvent.DateEnd.AddDays(1)), TimeOnly.MaxValue);
                    newEventEnd = newEventEnd < @event.DateEnd ? newEventEnd : @event.DateEnd;

                    if (newEventStart == newEventEnd) {
                        break;
                    }

                    subEvent = @event with {
                        DateStart = newEventStart,
                        DateEnd = newEventEnd
                    };

                    yield return subEvent;
                }

            }

        }
    }

    private int CountOverlaps(IEnumerable<Tasks> events) {
        if (events.Any()) {
            var overlaps = 0;
            var currentEvent = events.First();
            foreach (var @event in events.Skip(1)) {
                if (@event.DateStart < currentEvent.DateEnd) {
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


    internal override GridPosition GetEventPosition(Tasks task) {
        var rowIndex = _timeSlots[TimeOnly.FromDateTime(task.DateStart)].RowIndex;
        var columnIndex = _visibleDates[DateOnly.FromDateTime(task.DateStart)].ColumnIndex;
        int rowSpan;
        if (task.DateStart.Date == task.DateEnd.Date) {
            rowSpan = _timeSlots[TimeOnly.FromDateTime(task.DateEnd)].RowIndex - rowIndex;
        }
        else {
            rowSpan = _timeSlots.Last().Value.RowIndex - rowIndex + 1;
        }
        return new GridPosition() {
            ColumnIndex = columnIndex,
            RowIndex = rowIndex,
            RowSpan = rowSpan,
        };
    }

}
