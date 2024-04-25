using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TnTComponents.Core;
using TnTComponents.Ext;
using TnTComponents.Scheduler;

namespace TnTComponents;

public partial class TnTWeekView {

    public override string? CssClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-week-view")
        .Build();

    public override string? CssStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    [Parameter]
    public TimeSpan Interval { get; set; } = TimeSpan.FromMinutes(30);

    [Parameter]
    public DayOfWeek StartWeekDay { get; set; } = DayOfWeek.Sunday;

    [Inject]
    private IJSRuntime _jsRuntime { get; set; } = default!;

    private IDictionary<TimeOnly, ElementReference> _rows = new Dictionary<TimeOnly, ElementReference>();
    private ElementReference[] _columns = new ElementReference[7];

    protected override IEnumerable<TimeOnly> GetTimeSlots() {
        var currentTime = Scheduler.MinimumTime;

        while (currentTime >= Scheduler.MinimumTime && currentTime <= Scheduler.MaximumTime) {
            yield return currentTime;
            var newTime = currentTime.Add(Interval);
            if (newTime < currentTime) {
                break;
            }
            currentTime = newTime;
        }
    }

    protected override IEnumerable<DateOnly> GetVisibleDates() {
        var diff = StartWeekDay - Scheduler.StartDate.DayOfWeek;
        var startDate = Scheduler.StartDate.AddDays(diff);

        for (var i = 0; i < 7; i++) {
            yield return startDate.AddDays(i);
        }
    }

    internal override async Task<BoundingClientRect> CalculateEventBoundingRect(TnTEvent @event) {
        var startTime = TimeOnly.FromDateTime(@event.StartTime.DateTime).RoundToNearestMinuteInterval(Interval);
        var endTime = TimeOnly.FromDateTime(@event.StartTime.DateTime).RoundToNearestMinuteInterval(Interval);

        var startDate = DateOnly.FromDateTime(@event.StartTime.Date);
        var endDate = DateOnly.FromDateTime(@event.EndTime.Date);

        var startColumn = _columns[(int)startDate.DayOfWeek];
        var startRow = _rows[startTime];
        var endRow = _rows[endTime];
        var endColumn = _columns[(int)endDate.DayOfWeek];

        var a = await _jsRuntime.GetBoundingClientRect(startColumn);
        var b = await _jsRuntime.GetBoundingClientRect(startRow);
        var c = await _jsRuntime.GetBoundingClientRect(endRow);
        var d = await _jsRuntime.GetBoundingClientRect(endColumn);

        return new BoundingClientRect() {
            Top = b.Top,
            Height = d.Top - b.Top,
            Left = a.Left,
            Width = a.Width
        };
    }
}