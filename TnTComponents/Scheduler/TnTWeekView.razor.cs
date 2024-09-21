using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using System.Collections.Immutable;
using System.Text;
using TnTComponents.Core;
using TnTComponents.Scheduler;

namespace TnTComponents;

public partial class TnTWeekView<TEventType> where TEventType : TnTEvent {

    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-week-view")
        .Build();

    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddVariable("header-height", $"{_headerHeight}px")
        .AddVariable("cell-min-width", $"{_cellMinWidth}px")
        .AddVariable("cell-height", $"{_cellHeight}px")
        .AddVariable("time-column-width", $"{_timeColumnWidth}px")
        .AddVariable("hour-offset", $"{_hourOffset}px")
        .Build();

    [Parameter]
    public bool HideDates { get; set; }

    [Parameter]
    public DayOfWeek StartViewOn { get; set; } = DayOfWeek.Sunday;

    [Parameter]
    public bool HideEventDates { get; set; }

    private const int _cellHeight = 48;
    private const int _cellMinWidth = 80;
    private const int _headerHeight = 80;
    private const int _hourOffset = _cellHeight;
    private const int _timeColumnWidth = 36;
    private Dictionary<DateOnly, SortedSet<WeekViewTnTEvent>> _events = [];
    private ImmutableSortedSet<DateOnly> _visibleDates = [];

    public override DateOnly DecrementDate(DateOnly src) => src.AddDays(-7);

    public override DateOnly IncrementDate(DateOnly src) => src.AddDays(7);

    public override void Refresh() {
        UpdateVisibleDates();
        UpdateEventsList();
        StateHasChanged();
    }

    protected override DateTimeOffset CalculateDateTimeOffset(double pointerYOffset, DateOnly date) {
        var time = TimeOnly.FromTimeSpan(TimeSpan.FromHours(pointerYOffset / _cellHeight));
        return Floor(new DateTimeOffset(date, time, TimeZoneInfo.Local.GetUtcOffset(DateTimeOffset.UtcNow)), TimeSpan.FromMinutes(15));
    }

    protected override void OnInitialized() {
        base.OnInitialized();
        UpdateVisibleDates();
        UpdateEventsList();
    }

    private void UpdateEventsList() {
        _events.Clear();
        foreach (var @event in Scheduler.Events.Where(e => e.StartDate <= _visibleDates.Last() && e.EndDate >= _visibleDates.First()).OrderBy(e => e.EventStart)) {
            var eventStart = @event.StartDate >= _visibleDates.First() ? @event.EventStart : new DateTimeOffset(_visibleDates.First(), TimeOnly.MinValue, @event.EventStart.Offset);
            var eventEnd = @event.EndDate <= _visibleDates.Last() ? @event.EventEnd : new DateTimeOffset(_visibleDates.Last(), TimeOnly.MaxValue, @event.EventEnd.Offset);

            do {
                var entryEnd = new DateTimeOffset(Math.Min(eventEnd.Ticks, new DateTimeOffset(DateOnly.FromDateTime(eventStart.LocalDateTime), TimeOnly.MaxValue, eventStart.Offset).Ticks), eventStart.Offset);
                var entry = new WeekViewTnTEvent {
                    WeekViewEventEnd = entryEnd,
                    WeekViewEventStart = eventStart,
                    Event = @event
                };

                if (_events.TryGetValue(entry.StartDate, out var sortedList)) {
                    var lastEvent = sortedList.Last();
                    if (lastEvent.Event.Overlaps(entry.Event)) {
                        entry.OverlapCount = lastEvent.OverlapCount + 1;

                        if (Math.Abs((entry.StartTime - lastEvent.StartTime).Ticks) <= TimeSpan.FromMinutes(30).Ticks) {
                            lastEvent.HeaderOverlapCount ??= new HeaderOverlapCount { Count = 1 };
                            lastEvent.HeaderOverlapIndex ??= 0;
                            entry.HeaderOverlapIndex = lastEvent.HeaderOverlapIndex + 1;
                            entry.HeaderOverlapCount = lastEvent.HeaderOverlapCount;
                            entry.HeaderOverlapCount.Count++;
                        }
                    }

                    sortedList.Add(entry);
                }
                else {
                    _events.Add(entry.StartDate, new SortedSet<WeekViewTnTEvent>(new WeekViewTnTEventComparer()) { entry });
                }

                eventStart = new DateTimeOffset(DateOnly.FromDateTime(eventStart.LocalDateTime), TimeOnly.MinValue, eventStart.Offset).AddDays(1);
            }
            while (eventStart < eventEnd);
        }
    }

    private void UpdateVisibleDates() {
        var diff = (7 + (Scheduler.Date.DayOfWeek - StartViewOn)) % 7;
        var startOfWeek = Scheduler.Date.AddDays(-1 * diff);

        _visibleDates = Enumerable.Range(0, 7)
            .Select(startOfWeek.AddDays)
            .ToImmutableSortedSet();
    }

    private sealed record WeekViewTnTEvent {
        public required DateTimeOffset WeekViewEventStart { get; init; }
        public required DateTimeOffset WeekViewEventEnd { get; init; }
        public int OverlapCount { get; set; }
        public int? HeaderOverlapIndex { get; set; }
        public HeaderOverlapCount? HeaderOverlapCount { get; set; }
        public required TEventType Event { get; init; }

        public TimeOnly StartTime => TimeOnly.FromTimeSpan(WeekViewEventStart.LocalDateTime.TimeOfDay);
        public TimeOnly EndTime => TimeOnly.FromTimeSpan(WeekViewEventEnd.LocalDateTime.TimeOfDay);
        public DateOnly StartDate => DateOnly.FromDateTime(WeekViewEventStart.LocalDateTime.Date);
        public DateOnly EndDate => DateOnly.FromDateTime(WeekViewEventEnd.LocalDateTime.Date);
    }

    private sealed class HeaderOverlapCount {
        public int Count { get; set; }
    }

    private class WeekViewTnTEventComparer : IComparer<WeekViewTnTEvent> {

        public int Compare(WeekViewTnTEvent? x, WeekViewTnTEvent? y) {
            var result = x?.Event.StartTime.CompareTo(y?.Event.StartTime);
            if (result == 0) {
                result = x?.Event.Duration.CompareTo(y?.Event.Duration);
            }
            return result.GetValueOrDefault() == 0 ? 1 : result.GetValueOrDefault(1);
        }
    }
}