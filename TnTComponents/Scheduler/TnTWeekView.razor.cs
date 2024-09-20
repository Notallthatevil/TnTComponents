using Microsoft.AspNetCore.Components;
using System.Text;
using TnTComponents.Core;
using TnTComponents.Scheduler;

namespace TnTComponents;

public partial class TnTWeekView<TEventType> where TEventType : TnTEvent {

    [Parameter]
    public DayOfWeek StartViewOn { get; set; } = DayOfWeek.Sunday;

    private IEnumerable<DateOnly> _visibleDates = [];


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
    }

    private void UpdateVisibleDates() {
        var diff = (7 + (Scheduler.Date.DayOfWeek - StartViewOn)) % 7;
        var startOfWeek = Scheduler.Date.AddDays(-1 * diff);

        _visibleDates = Enumerable.Range(0, 7)
            .Select(startOfWeek.AddDays);
    }

    public override void Refresh() {
        UpdateVisibleDates();
        StateHasChanged();
    }
}

