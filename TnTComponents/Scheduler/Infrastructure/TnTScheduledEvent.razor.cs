using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TnTComponents.Ext;

namespace TnTComponents.Scheduler.Infrastructure;
public partial class TnTScheduledEvent {
    [Parameter, EditorRequired]
    public TnTEvent Event { get; set; } = default!;

    [Parameter, EditorRequired]
    public ScheduleViewBase ScheduleView { get; set; } = default!;

    public override string? JsModulePath => "./_content/TnTComponents/Scheduler/Infrastructure/TnTScheduledEvent.razor.js";

    public override string? CssClass => null;
    public override string? CssStyle => null;

    protected override void OnParametersSet() {
        base.OnParametersSet();
        if (Event is null) {
            throw new InvalidOperationException($"The {GetType().Name} component requires a {nameof(TnTEvent)}");
        }

        if (ScheduleView is null) {
            throw new InvalidOperationException($"The {GetType().Name} component requires a {nameof(ScheduleViewBase)}");
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        await base.OnAfterRenderAsync(firstRender);
        await SizeAndPositionElement();
    }

    [JSInvokable]
    public async Task SizeAndPositionElement() {
        var boundingRect = await ScheduleView.CalculateEventBoundingRect(Event);
        await JSRuntime.SetBoundingClientRectAsync(Element, boundingRect);
    }
}
