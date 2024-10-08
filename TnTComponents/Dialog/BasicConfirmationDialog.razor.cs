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

    public static async Task<DialogResult> OpenForResultAsync(ITnTDialogService dialogService, string title, string body, string? confirmButtonText = "Confirm", string? cancelButtonText = "Cancel", EventCallback? confirmClickedCallback = null, EventCallback? cancelClickedCallback = null,
        TnTBorderRadius? borderRadius = null,
        TnTColor dialogBackgroundColor = TnTColor.SurfaceContainer,
        int elevation = 1,
        bool overlayBlur = true,
        TnTColor overlayColor = TnTColor.OnSurfaceVariant,
        double? overlayOpacity = 0.5,
        string? style = null,
        TnTColor textColor = TnTColor.OnSurface,
        bool showCancelButton = true) {
        var options = new TnTDialogOptions() {
            BorderRadius = borderRadius ?? new(3),
            CloseOnExternalClick = false,
            ShowCloseButton = false,
            Title = title,
            Elevation = elevation,
            ElementStyle = style,
            TextColor = textColor
        };

        return await dialogService.OpenForResultAsync<BasicConfirmationDialog>(options,
            new Dictionary<string, object?> {
                { nameof(Body), body },
                { nameof(ConfirmClickedCallback), confirmClickedCallback ?? default },
                { nameof(CancelClickedCallback), cancelClickedCallback ?? default },
                { nameof(ConfirmButtonText), confirmButtonText },
                { nameof(CancelButtonText), cancelButtonText },
                { nameof(CancelButtonTextColor), textColor },
                 { nameof(ShowCancelButton), showCancelButton }
            });
    }

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