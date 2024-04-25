using BlazorCalendar;
using BlazorCalendar.Models;
using Microsoft.AspNetCore.Components;
using TnTComponents.Scheduler;

namespace TnTComponents;

[CascadingTypeParameter(nameof(TEventType))]
partial class TnTScheduler<TEventType> : CalendarBase where TEventType : TnTEvent {
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
