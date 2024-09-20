using Microsoft.AspNetCore.Components;
using System.Collections.Immutable;
using System.Text;
using TnTComponents.Core;
using TnTComponents.Scheduler;

namespace TnTComponents;

public partial class TnTWeekView<TEventType> where TEventType : TnTEvent {

    [Parameter]
    public DayOfWeek StartViewOn { get; set; } = DayOfWeek.Sunday;

    private ImmutableSortedSet<DateOnly> _visibleDates = [];

    private Dictionary<DateOnly, SortedSet<WeekViewTnTEvent>> _events = [];


    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-week-view")
        .Build();

    public override string? ElementStyle => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    public override DateOnly DecrementDate(DateOnly src) => src.AddDays(-7);
    public override DateOnly IncrementDate(DateOnly src) => src.AddDays(7);

    protected override void OnInitialized() {
        base.OnInitialized();
        UpdateVisibleDates();
        UpdateEventsList();
    }

    private void UpdateVisibleDates() {
        var diff = (7 + (Scheduler.Date.DayOfWeek - StartViewOn)) % 7;
        var startOfWeek = Scheduler.Date.AddDays(-1 * diff);

        _visibleDates = Enumerable.Range(0, 7)
            .Select(startOfWeek.AddDays)
            .ToImmutableSortedSet();
    }

    private void UpdateEventsList() {
        _events.Clear();
        foreach (var @event in Scheduler.Events.Where(e => e.StartDate <= _visibleDates.Last() && e.EndDate >= _visibleDates.First()).OrderBy(e=>e.EventStart)) {
            var eventStart = @event.StartDate >= _visibleDates.First() ? @event.EventStart : new DateTimeOffset(_visibleDates.First(), TimeOnly.MinValue, @event.EventStart.Offset);
            var eventEnd = @event.EndDate <= _visibleDates.Last() ? @event.EventEnd : new DateTimeOffset(_visibleDates.Last(), TimeOnly.MaxValue, @event.EventEnd.Offset);

            do {
                var entryEnd = new DateTimeOffset(Math.Min(eventEnd.Ticks, new DateTimeOffset(DateOnly.FromDateTime(eventStart.LocalDateTime), TimeOnly.MaxValue, eventStart.Offset).Ticks), eventStart.Offset);
                var entry = new WeekViewTnTEvent {
                    BackgroundColor = @event.BackgroundColor,
                    Description = @event.Description,
                    EventEnd = entryEnd,
                    EventStart = eventStart,
                    ForegroundColor = @event.ForegroundColor,
                    Id = @event.Id,
                    OriginalEventEnd = @event.EventEnd,
                    OriginalEventStart = @event.EventStart,
                    Title = @event.Title
                };

                if (_events.TryGetValue(entry.StartDate, out var sortedList)) {
                    var lastEvent = sortedList.Last();
                    if (lastEvent.Overlaps(entry)) {
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
                    _events.Add(entry.StartDate, new SortedSet<WeekViewTnTEvent>(new DuplicateKeyComparer()) {  entry  });
                }

                eventStart = new DateTimeOffset(DateOnly.FromDateTime(eventStart.LocalDateTime), TimeOnly.MinValue, eventStart.Offset).AddDays(1);
            }
            while (eventStart < eventEnd);
        }
    }

    public override void Refresh() {
        UpdateVisibleDates();
        UpdateEventsList();
        StateHasChanged();
    }


    private sealed record WeekViewTnTEvent : TnTEvent {
        public required DateTimeOffset OriginalEventStart { get; init; }
        public required DateTimeOffset OriginalEventEnd { get; init; }
        public int OverlapCount { get; set; }
        public int? HeaderOverlapIndex { get; set; }
        public HeaderOverlapCount? HeaderOverlapCount { get; set; }
    }

    private sealed class HeaderOverlapCount {
        public int Count { get; set; }
    }

    /// <summary>
    /// Comparer for comparing two keys, handling equality as beeing greater
    /// Use this Comparer e.g. with SortedLists or SortedDictionaries, that don't allow duplicate keys
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    private class DuplicateKeyComparer : IComparer<WeekViewTnTEvent> {
        public int Compare(WeekViewTnTEvent? x, WeekViewTnTEvent? y) {
            var result = x?.StartTime.CompareTo(y?.StartTime);
            if(result == 0) {
                result = x?.Duration.CompareTo(y?.Duration);
            }
            return result.GetValueOrDefault() == 0 ? 1 : result.GetValueOrDefault(1);
        }
    }
}
