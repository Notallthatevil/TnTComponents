using Microsoft.AspNetCore.Components;
using TnTComponents.Dialog;
using TnTComponents.Snackbar;

namespace TnTComponents.Scheduler.Infrastructure;

public partial class AddEventDialog<TEventType> where TEventType : TnTEvent, new() {
    [CascadingParameter]
    private ITnTDialog _dialog { get; set; } = default!;

    [Parameter]
    public RenderFragment<TEventType>? AddEventDialogTemplate { get; set; }

    [Parameter, EditorRequired]
    public TEventType Event { get; set; } = default!;

    [Parameter]
    public EventCallback<TEventType> OnSaveCallback { get; set; }

    protected override void OnParametersSet() {
        base.OnParametersSet();
        ArgumentNullException.ThrowIfNull(OnSaveCallback, nameof(OnSaveCallback));
    }

    private async Task SaveEventAsync() {
        await OnSaveCallback.InvokeAsync(Event);
        await _dialog.CloseAsync();
    }

    private void UpdateEndDate(DateTimeOffset? startDate) {
        if (startDate.HasValue && Event.End.HasValue && startDate.Value >= Event.End.Value) {
            Event.End = startDate.Value.AddMinutes(30);
        }
    }
}