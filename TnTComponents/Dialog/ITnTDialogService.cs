using Microsoft.AspNetCore.Components;
using TnTComponents.Dialog;

namespace TnTComponents;

public interface ITnTDialogService {

    event OnCloseCallback? OnClose;

    event OnOpenCallback? OnOpen;

    delegate Task OnCloseCallback(ITnTDialog dialog);
    delegate Task OnOpenCallback(ITnTDialog dialog);
    Task CloseAsync(ITnTDialog dialog);

    Task<ITnTDialog> OpenAsync<TComponent>(TnTDialogOptions? options = null, IReadOnlyDictionary<string, object?>? parameters = null) where TComponent : IComponent;

    Task<ITnTDialog> OpenAsync(RenderFragment renderFragment, TnTDialogOptions? options = null);

    Task<DialogResult> OpenForResultAsync<TComponent>(TnTDialogOptions? options = null, IReadOnlyDictionary<string, object?>? parameters = null) where TComponent : IComponent;

    Task<DialogResult> OpenForResultAsync(RenderFragment renderFragment, TnTDialogOptions? options = null);
}

public static class ITnTDialogServiceExt {
    public static Task<ITnTDialog> OpenConfirmationDialogAsync(this ITnTDialogService dialogService,
        EventCallback confirmationCallback,
        string confirmButtonText = "Confirm",
        string body = "Are you sure?",
        string cancelButtonText = "Cancel",
        TnTColor cancelButtonTextColor = TnTColor.Surface,
        EventCallback cancellationCallback = default,
        bool showCancelButton = true,
        TnTDialogOptions? options = null) =>
        dialogService.OpenAsync<BasicConfirmationDialog>(options, new Dictionary<string, object?> {
            { nameof(BasicConfirmationDialog.Body), body },
            { nameof(BasicConfirmationDialog.CancelButtonText), cancelButtonText },
            { nameof(BasicConfirmationDialog.CancelButtonTextColor), cancelButtonTextColor },
            { nameof(BasicConfirmationDialog.CancelClickedCallback), cancellationCallback },
            { nameof(BasicConfirmationDialog.ConfirmButtonText), confirmButtonText },
            { nameof(BasicConfirmationDialog.ConfirmClickedCallback), confirmationCallback },
            { nameof(BasicConfirmationDialog.ShowCancelButton), showCancelButton }
        });

    public static Task<DialogResult> OpenConfirmationDialogForResultAsync(this ITnTDialogService dialogService,
        EventCallback confirmationCallback,
        string confirmButtonText = "Confirm",
        string body = "Are you sure?",
        string cancelButtonText = "Cancel",
        TnTColor cancelButtonTextColor = TnTColor.Surface,
        EventCallback cancellationCallback = default,
        bool showCancelButton = true,
        TnTDialogOptions? options = null) =>
        dialogService.OpenForResultAsync<BasicConfirmationDialog>(options, new Dictionary<string, object?> {
            { nameof(BasicConfirmationDialog.Body), body },
            { nameof(BasicConfirmationDialog.CancelButtonText), cancelButtonText },
            { nameof(BasicConfirmationDialog.CancelButtonTextColor), cancelButtonTextColor },
            { nameof(BasicConfirmationDialog.CancelClickedCallback), cancellationCallback },
            { nameof(BasicConfirmationDialog.ConfirmButtonText), confirmButtonText },
            { nameof(BasicConfirmationDialog.ConfirmClickedCallback), confirmationCallback },
            { nameof(BasicConfirmationDialog.ShowCancelButton), showCancelButton }
        });
}