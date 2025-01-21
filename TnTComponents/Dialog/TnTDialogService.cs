using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using TnTComponents.Core;

namespace TnTComponents.Dialog;

/// <summary>
///     Service for managing dialogs in the application.
/// </summary>
internal class TnTDialogService : ITnTDialogService {

    public event ITnTDialogService.OnCloseCallback? OnClose;

    public event ITnTDialogService.OnOpenCallback? OnOpen;

    /// <summary>
    ///     Gets the reference to the current instance of the dialog service.
    /// </summary>
    internal DotNetObjectReference<TnTDialogService> Reference { get; private set; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TnTDialogService" /> class.
    /// </summary>
    public TnTDialogService() => Reference = DotNetObjectReference.Create(this);

    public async Task CloseAsync(ITnTDialog dialog) => await (OnClose?.Invoke(dialog) ?? Task.CompletedTask);

    public async Task<ITnTDialog> OpenAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TComponent>(TnTDialogOptions? options = null, IReadOnlyDictionary<string, object?>? parameters = null) where TComponent : IComponent {
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
        return await OpenAsync<DeferRendering>(options, new Dictionary<string, object?> {
            { nameof(DeferRendering.ChildContent), renderFragment }
        });
    }

    public async Task<DialogResult> OpenForResultAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TComponent>(TnTDialogOptions? options = null, IReadOnlyDictionary<string, object?>? parameters = null) where TComponent : IComponent {
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
        return await OpenForResultAsync<DeferRendering>(options, new Dictionary<string, object?> {
            { nameof(DeferRendering.ChildContent), renderFragment }
        });
    }

    /// <summary>
    ///     Implementation of the <see cref="ITnTDialog" /> interface.
    /// </summary>
    private class DialogImpl(TnTDialogService dialogService) : ITnTDialog {
        public DialogResult DialogResult { get; set; }

        public string ElementId { get; init; } = TnTComponentIdentifier.NewId();

        public TnTDialogOptions Options { get; init; } = default!;

        public IReadOnlyDictionary<string, object?>? Parameters { get; init; }

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
        public Type Type { get; init; } = default!;

        public Task CloseAsync() => dialogService.CloseAsync(this);
    }
}