using Microsoft.AspNetCore.Components;
using TnTComponents.Core;
using TnTComponents.Dialog;

namespace TnTComponents;

public partial class BasicConfirmationDialog {

    [Parameter, EditorRequired]
    public string Body { get; set; } = default!;

    [Parameter]
    public string CancelButtonText { get; set; } = "Cancel";

    [Parameter]
    public TnTColor CancelButtonTextColor { get; set; } = TnTColor.OnSurface;

    [Parameter]
    public EventCallback CancelClickedCallback { get; set; }

    [Parameter]
    public string ConfirmButtonText { get; set; } = "Confirm";

    [Parameter]
    public EventCallback ConfirmClickedCallback { get; set; }

    [Parameter]
    public bool ShowCancelButton { get; set; } = true;

    [CascadingParameter]
    private ITnTDialog _dialog { get; set; } = default!;

    [Inject]
    private ITnTDialogService _dialogService { get; set; } = default!;

    protected override void OnParametersSet() {
        base.OnParametersSet();
        if (_dialog is null) {
            throw new InvalidOperationException($"{nameof(BasicConfirmationDialog)} must be created inside a dialog.");
        }
    }

    private async Task CancelClicked() {
        _dialog.DialogResult = DialogResult.Cancelled;
        await _dialog.CloseAsync();
        await CancelClickedCallback.InvokeAsync();
    }

    private async Task ConfirmClicked() {
        _dialog.DialogResult = DialogResult.Confirmed;
        await _dialog.CloseAsync();
        await ConfirmClickedCallback.InvokeAsync();
    }
}