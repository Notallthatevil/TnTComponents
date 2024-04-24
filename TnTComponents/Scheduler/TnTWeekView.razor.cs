
using Microsoft.AspNetCore.Components;
using TnTComponents.Core;

namespace TnTComponents;
public partial class TnTWeekView {
    [Parameter]
    public DayOfWeek StartWeekDay { get; set; } = DayOfWeek.Sunday;

    [Parameter]
    public TimeSpan Interval { get; set; } = TimeSpan.FromMinutes(30);

    public override string? CssClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-week-view")
        .Build();

    public override string? CssStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    protected override IEnumerable<DateOnly> GetVisibleDates() {
        var startDate = Scheduler.StartDate.AddDays(((int)StartWeekDay) * -1);

        for (var i = 0; i < 7; i++) {
            yield return startDate.AddDays(i);
        }
    }

    protected override IEnumerable<TimeOnly> GetTimeSlots() {
        var currentTime = Scheduler.MinimumTime;

        while (currentTime >= Scheduler.MinimumTime && currentTime <= Scheduler.MaximumTime) {
            yield return currentTime;
            var newTime = currentTime.Add(Interval);
            if(newTime < currentTime) {
                break;
            }
            currentTime = newTime;
        }
    }
}
