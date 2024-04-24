using Microsoft.AspNetCore.Components;
using TnTComponents.Core;

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
}