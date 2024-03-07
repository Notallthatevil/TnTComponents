using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TnTComponents.Enum;

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

    public async Task OpenAsync<TComponent>(TnTDialogOptions? options = null, IReadOnlyDictionary<string, object>? parameters = null) where TComponent : IComponent =>
        await (OnOpen?.Invoke(new DialogImpl(this) {
            Options = options ?? new(),
            Parameters = parameters,
            Type = typeof(TComponent)
        }
        ) ?? Task.CompletedTask);

    public async Task OpenForResultAsync<TComponent>(EventCallback<DialogResult> resultCallback, TnTDialogOptions? options = null, IReadOnlyDictionary<string, object>? parameters = null) where TComponent : IComponent =>
     await (OnOpen?.Invoke(new DialogImpl(this, resultCallback) {
         Options = options ?? new(),
         Parameters = parameters,
         Type = typeof(TComponent)
     }
     ) ?? Task.CompletedTask);

    public async Task CloseAsync(ITnTDialog dialog) => await (OnClose?.Invoke(dialog) ?? Task.CompletedTask);

    private class DialogImpl(TnTDialogService dialogService, EventCallback<DialogResult> resultCallback = default) : ITnTDialog {
        public TnTDialogOptions Options { get; init; } = default!;
        public IReadOnlyDictionary<string, object>? Parameters { get; init; }
        public Type Type { get; init; } = default!;
        public DialogResult DialogResult { get; set; }

        public Task CloseAsync() {
            return dialogService.CloseAsync(this);
        }

        public async Task Closing() {
            await resultCallback.InvokeAsync(DialogResult);
        }

    }
}