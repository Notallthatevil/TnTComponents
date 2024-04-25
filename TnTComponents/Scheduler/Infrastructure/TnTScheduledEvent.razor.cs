using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TnTComponents.Ext;

namespace TnTComponents.Scheduler.Infrastructure;
public partial class TnTScheduledEvent {
    [Parameter, EditorRequired]
    public TnTEvent Event { get; set; } = default!;

    [Parameter, EditorRequired]
    public ScheduleViewBase ScheduleView { get; set; } = default!;

    private ElementReference _element;

    [Inject]
    private IJSRuntime _jsRuntime { get; set; } = default!;

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
        var boundingRect = await ScheduleView.CalculateEventBoundingRect(Event);
        await _jsRuntime.SetBoundingClientRectAsync(_element, boundingRect);
        await _jsRuntime.SetOpacity(_element, 1);
    }
}
