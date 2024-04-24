using Microsoft.AspNetCore.Components;
using TnTComponents.Core;
using TnTComponents.Dialog;
using TnTComponents.Ext;
using TnTComponents.Scheduler;
using TnTComponents.Scheduler.Infrastructure;

namespace TnTComponents;

public partial class TnTWeekView<TEventType> where TEventType : TnTEvent, new() {

    [Parameter]
    public RenderFragment<TEventType>? AddEventDialogTemplate { get; set; }

    public override string? CssClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-schedule-view")
        .AddClass("tnt-week-view")
        .Build();

    public override string? CssStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    [Parameter]
    public IEnumerable<DisabledTime> DisabledTimes { get; set; } = [];

    public override string? JsModulePath => "./_content/TnTComponents/Scheduler/TnTWeekView.razor.js";

    [Parameter]
    public DayOfWeek StartWeekDay { get; set; } = DayOfWeek.Sunday;

    [Parameter]
    public bool WeekViewOnly { get; set; }

    private ElementReference[] _columnHeaders = new ElementReference[7];


    protected override IEnumerable<TEventType> GetVisibleEvents() {
        var startDate = new DateTime(Scheduler.StartDate, default);
        var endDate = new DateTime(Scheduler.StartDate.AddDays(7), default);
        return Scheduler.Events.Where(x => x.Start?.DateTime < endDate && startDate < x.End?.DateTime);
    }

    private async Task AddEventAsync(DateOnly day, TimeOnly time) {
        var options = new TnTDialogOptions() {
            Title = "New Event"
        };

        var @event = new TEventType() {
            Start = new DateTimeOffset(day, time, DateTimeOffset.Now.Offset),
            Title = string.Empty
        };
        @event.End = @event.Start?.AddMinutes(30);

        await DialogService.OpenAsync<AddEventDialog<TEventType>>(options,
                 new Dictionary<string, object?> {
                    { nameof(AddEventDialog<TEventType>.Event), @event },
                    { nameof(AddEventDialog<TEventType>.AddEventDialogTemplate), AddEventDialogTemplate },
                    { nameof(AddEventDialog<TEventType>.OnSaveCallback), EventCallback.Factory.Create<TEventType>(this, Scheduler.AddEvent) }
                 }
             );
    }

    private IEnumerable<DateOnly> GetDaysOfWeek() {
        var diff = Math.Abs(Scheduler.StartDate.DayOfWeek - StartWeekDay);

        var startDate = Scheduler.StartDate.AddDays(-diff);
        var currentDate = startDate;
        while (currentDate < startDate.AddDays(7)) {
            yield return currentDate;
            currentDate = currentDate.AddDays(1);
        }
    }

    private bool IsDisabledTime(DayOfWeek day, TimeOnly time) {
        return DisabledTimes.Any(x => x.StartTime <= time && x.EndTime > time && x.DayOfWeek.HasDay(day));
    }
}