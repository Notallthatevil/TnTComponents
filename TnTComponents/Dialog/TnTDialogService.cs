using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TnTComponents.Dialog.Infrastructure;

namespace TnTComponents.Dialog;

internal class TnTDialogService : ITnTDialogService {

    public event ITnTDialogService.OnCloseCallback? OnClose;

    public event ITnTDialogService.OnOpenCallback? OnOpen;

    internal DotNetObjectReference<TnTDialogService> Reference { get; private set; }

    public TnTDialogService() {
        Reference = DotNetObjectReference.Create(this);
    }

    public async Task CloseAsync(ITnTDialog dialog) => await (OnClose?.Invoke(dialog) ?? Task.CompletedTask);

    public async Task<ITnTDialog> OpenAsync<TComponent>(TnTDialogOptions? options = null, IReadOnlyDictionary<string, object?>? parameters = null) where TComponent : IComponent {
        var dialog = new DialogImpl(this) {
            Options = options ?? new(),
            Parameters = parameters,
            Type = typeof(TComponent)
        };

        await (OnOpen?.Invoke(dialog) ?? Task.CompletedTask);
        return dialog;
    }

    public async Task<ITnTDialog> OpenAsync(RenderFragment renderFragment, TnTDialogOptions? options = null) {
        ArgumentNullException.ThrowIfNull(renderFragment, nameof(renderFragment));
        return await OpenAsync<DialogHelperComponent>(options, new Dictionary<string, object?> {
            { nameof(DialogHelperComponent.ChildContent), renderFragment }
        });
    }

    public async Task<DialogResult> OpenForResultAsync<TComponent>(TnTDialogOptions? options = null, IReadOnlyDictionary<string, object?>? parameters = null) where TComponent : IComponent {
        var dialog = new DialogImpl(this) {
            Options = options ?? new(),
            Parameters = parameters,
            Type = typeof(TComponent)
        };
        if (OnOpen is not null) {
            await OnOpen.Invoke(dialog);
            while (dialog.DialogResult == DialogResult.Pending) {
                await Task.Delay(500);
            }
            return dialog.DialogResult;
        }
        else {
            return DialogResult.Failed;
        }
    }

    public async Task<DialogResult> OpenForResultAsync(RenderFragment renderFragment, TnTDialogOptions? options = null) {
        ArgumentNullException.ThrowIfNull(renderFragment, nameof(renderFragment));
        return await OpenForResultAsync<DialogHelperComponent>(options, new Dictionary<string, object?> {
            { nameof(DialogHelperComponent.ChildContent), renderFragment }
        });
    }

    private class DialogImpl(TnTDialogService dialogService) : ITnTDialog {
        public DialogResult DialogResult { get; set; }
        public TnTDialogOptions Options { get; init; } = default!;
        public IReadOnlyDictionary<string, object?>? Parameters { get; init; }
        public Type Type { get; init; } = default!;

        public Task CloseAsync() {
            return dialogService.CloseAsync(this);
        }
    }
}