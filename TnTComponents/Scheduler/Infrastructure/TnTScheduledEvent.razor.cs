using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TnTComponents.Core;

namespace TnTComponents.Scheduler.Infrastructure;
public partial class TnTScheduledEvent<TEventType> where TEventType : TnTEvent, new() {

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter, EditorRequired]
    public TEventType Event { get; set; } = default!;

    [Parameter]
    public RenderFragment<TEventType>? EventTemplate { get; set; }

    [Inject]
    private IJSRuntime _jsRuntime { get; set; } = default!;

    [CascadingParameter]
    private TnTScheduleView<TEventType> _scheduleView { get; set; } = default!;
    public string? CssStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    protected override void OnParametersSet() {
        base.OnParametersSet();
        ArgumentNullException.ThrowIfNull(Event, nameof(Event));
        if(_scheduleView is null) {
            throw new ArgumentNullException(nameof(_scheduleView), $"{nameof(TnTScheduledEvent<TEventType>)} must be a child of {nameof(TnTScheduleView<TEventType>)}");
        }
    }
}
