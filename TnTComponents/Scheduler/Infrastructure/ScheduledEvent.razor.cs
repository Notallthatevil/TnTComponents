using BlazorCalendar.Models;
using Microsoft.AspNetCore.Components;
using TnTComponents.Core;

namespace TnTComponents.Scheduler.Infrastructure;
public partial class ScheduledEvent<TEventType> where TEventType : TnTEvent {
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public bool? AutoFocus { get; set; }

    public string? CssClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-slot")
        .AddClass("tnt-event")
        .AddBorderRadius(BorderRadius)
        .AddElevation(Elevation)
        .Build();

    [Parameter]
    public bool Disabled { get; set; }

    public ElementReference Element { get; private set; }
    public string? Id { get; } = TnTComponentIdentifier.NewId();

    public string? CssStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    [Parameter, EditorRequired]
    public Tasks Event { get; set; } = default!;

    [Parameter, EditorRequired]
    public ScheduleViewBase<TEventType> ScheduleView { get; set; } = default!;

    [Parameter]
    public TnTBorderRadius? BorderRadius { get; set; } = new(1);
    [Parameter]
    public int Elevation { get; set; } = 1;

    protected override void OnParametersSet() {
        base.OnParametersSet();
        if (Event is null) {
            throw new InvalidOperationException($"{GetType().Name} requires a parameter of type {nameof(Tasks)}.");
        }
        if (ScheduleView is null) {
            throw new InvalidOperationException($"{GetType().Name} requires a parameter of type {nameof(ScheduleViewBase<TEventType>)}.");
        }
    }


    private string? EventPositionStyle(Tasks @event) {
        var position = ScheduleView.GetEventPosition(@event);
        return CssStyleBuilder.Create()
            .AddVariable("row-index", position.RowIndex.ToString())
            .AddVariable("column-index", position.ColumnIndex.ToString())
            .AddVariable("row-span", position.RowSpan.ToString())
            .AddVariable("column-span", position.ColumnSpan.ToString())
            .Build();
    }

}
