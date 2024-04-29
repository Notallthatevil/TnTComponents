using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace TnTComponents.Dialog;

public class TnTDialogService {

    public event OnCloseCallback? OnClose;

    public delegate Task OnCloseCallback(ITnTDialog dialog);

    public event OnOpenCallback? OnOpen;

    public delegate Task OnOpenCallback(ITnTDialog dialog);

    internal DotNetObjectReference<TnTDialogService> Reference { get; private set; }

    public TnTDialogService() {
        Reference = DotNetObjectReference.Create(this);
    }

    public async Task CloseAsync(ITnTDialog dialog) => await (OnClose?.Invoke(dialog) ?? Task.CompletedTask);

    public async Task OpenAsync<TComponent>(TnTDialogOptions? options = null, IReadOnlyDictionary<string, object?>? parameters = null) where TComponent : IComponent =>
            await (OnOpen?.Invoke(new DialogImpl(this) {
                Options = options ?? new(),
                Parameters = parameters,
                Type = typeof(TComponent)
            }
        ) ?? Task.CompletedTask);

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