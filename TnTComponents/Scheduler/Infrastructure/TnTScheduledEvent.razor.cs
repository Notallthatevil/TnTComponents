using Microsoft.AspNetCore.Components;

namespace TnTComponents.Scheduler.Infrastructure;
public partial class TnTScheduledEvent<TEventType> where TEventType : TnTEvent, new() {

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter, EditorRequired]
    public TEventType Event { get; set; } = default!;

    [Parameter]
    public RenderFragment<TEventType>? EventTemplate { get; set; }

    protected override void OnParametersSet() {
        base.OnParametersSet();
        ArgumentNullException.ThrowIfNull(Event, nameof(Event));
    }


}
