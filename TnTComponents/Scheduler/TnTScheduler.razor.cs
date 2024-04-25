using BlazorCalendar;
using BlazorCalendar.Models;
using Microsoft.AspNetCore.Components;
using TnTComponents.Core;
using TnTComponents.Scheduler;

namespace TnTComponents;

[CascadingTypeParameter(nameof(TEventType))]
partial class TnTScheduler<TEventType> : CalendarBase where TEventType : TnTEvent {

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    public string? CssClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-scheduler")
        .Build();

    public string? CssStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    public ElementReference Element { get; private set; }

    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //








    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public DateTime FirstDate { get; set; }

    [Parameter]
    public DisplayedView DisplayedView { get; set; }

    [Parameter]
    public Tasks[]? TasksList { get; set; }

    [Parameter]
    public bool DisableDragAndDrop { get; set; }
}
