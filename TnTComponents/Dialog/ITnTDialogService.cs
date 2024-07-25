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