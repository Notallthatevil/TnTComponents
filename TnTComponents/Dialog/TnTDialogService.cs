using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace TnTComponents.Dialog;

public class TnTDialogService {

    public delegate Task OnCloseCallback(ITnTDialog dialog);

    public delegate Task OnOpenCallback(ITnTDialog dialog);

    public event OnCloseCallback? OnClose;

    public event OnOpenCallback? OnOpen;

    internal DotNetObjectReference<TnTDialogService> Reference { get; private set; }

    public TnTDialogService() {
        Reference = DotNetObjectReference.Create(this);
    }

    private class DialogImpl : ITnTDialog {
        public TnTDialogOptions Options { get; init; } = default!;
        public IReadOnlyDictionary<string, object>? Parameters { get; init; }
        public Type Type { get; init; } = default!;
    }

    public async Task OpenAsync<TComponent>(TnTDialogOptions? options = null, IReadOnlyDictionary<string, object>? parameters = null) where TComponent : IComponent =>
        await (OnOpen?.Invoke(new DialogImpl() {
            Options = options ?? new(),
            Parameters = parameters,
            Type = typeof(TComponent)
        }
        ) ?? Task.CompletedTask);

    internal async Task CloseAsync(ITnTDialog dialog) => await (OnClose?.Invoke(dialog) ?? Task.CompletedTask);

}