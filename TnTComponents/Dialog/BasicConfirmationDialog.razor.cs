using Microsoft.AspNetCore.Components;
using TnTComponents.Core;
using TnTComponents.Dialog;

namespace TnTComponents;

/// <summary>
///     A customizable confirmation dialog component with confirm and cancel options. This component must be used within a dialog context.
/// </summary>
public partial class BasicConfirmationDialog {

    /// <summary>
    ///     The main message text displayed in the dialog body.
    /// </summary>
    [Parameter, EditorRequired]
    public string Body { get; set; } = default!;

    /// <summary>
    ///     The text displayed on the cancel button. Default value is "Cancel".
    /// </summary>
    [Parameter]
    public string CancelButtonText { get; set; } = "Cancel";

    /// <summary>
    ///     The color of the cancel button text. Default value is TnTColor.OnSurface.
    /// </summary>
    [Parameter]
    public TnTColor CancelButtonTextColor { get; set; } = TnTColor.OnSurface;

    /// <summary>
    ///     The callback that will be invoked when the cancel button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback CancelClickedCallback { get; set; }

    /// <summary>
    ///     The text displayed on the confirm button. Default value is "Confirm".
    /// </summary>
    [Parameter]
    public string ConfirmButtonText { get; set; } = "Confirm";

    /// <summary>
    ///     The callback that will be invoked when the confirm button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback ConfirmClickedCallback { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the cancel button should be displayed. Default value is true.
    /// </summary>
    [Parameter]
    public bool ShowCancelButton { get; set; } = true;

    /// <summary>
    ///     The dialog instance this component is contained within.
    /// </summary>
    [CascadingParameter]
    private ITnTDialog _dialog { get; set; } = default!;

    /// <inheritdoc />
    protected override void OnParametersSet() {
        base.OnParametersSet();
        if (_dialog is null) {
            throw new InvalidOperationException($"{nameof(BasicConfirmationDialog)} must be created inside a dialog.");
        }
    }

    /// <summary>
    ///     Handles the cancel button click event, sets the dialog result to cancelled, closes the dialog, and invokes the cancel callback if provided.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task CancelClicked() {
        _dialog.DialogResult = DialogResult.Cancelled;
        await _dialog.CloseAsync();
        await CancelClickedCallback.InvokeAsync();
    }

    /// <summary>
    ///     Handles the confirm button click event, sets the dialog result to confirmed, closes the dialog, and invokes the confirm callback if provided.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task ConfirmClicked() {
        _dialog.DialogResult = DialogResult.Confirmed;
        await _dialog.CloseAsync();
        await ConfirmClickedCallback.InvokeAsync();
    }
}