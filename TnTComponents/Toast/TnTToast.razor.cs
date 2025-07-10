using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Collections.Concurrent;
using TnTComponents.Core;
using TnTComponents.Toast;
using static TnTComponents.Toast.TnTToastService;

namespace TnTComponents;

/// <summary>
///     Represents a toast notification component that can display multiple toasts.
/// </summary>
public partial class TnTToast {

    /// <summary>
    ///     Gets or sets the toast service used to manage toasts.
    /// </summary>
    [Inject]
    private ITnTToastService _service { get; set; } = default!;

    private readonly ConcurrentDictionary<ITnTToast, ToastMetadata> _toasts = new();

    private readonly CancellationTokenSource _tokenSource = new();

    private const int _closeDelay = 250;

    /// <summary>
    ///     Disposes the resources used by the component.
    /// </summary>
    public void Dispose() {
        _tokenSource.Cancel();
        _tokenSource.Dispose();
        _service.OnClose -= OnClose;
        _service.OnOpen -= OnOpen;
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    protected override void OnInitialized() {
        base.OnInitialized();
        _service.OnOpen += OnOpen;
        _service.OnClose += OnClose;
    }

    /// <summary>
    ///     Handles the close event for a toast.
    /// </summary>
    /// <param name="toast">The toast to close.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnClose(ITnTToast toast) {
        if (toast is TnTToastImplementation impl) {
            impl.Closing = true;
        }
        StateHasChanged();
        await Task.Delay(_closeDelay);

        _toasts.Remove(toast, out _);

        StateHasChanged();
    }

    /// <summary>
    ///     Handles the open event for a toast.
    /// </summary>
    /// <param name="toast">The toast to open.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private Task OnOpen(ITnTToast toast) {
        _toasts.TryAdd(toast, new ToastMetadata() { CreatedTime = DateTimeOffset.Now, Task = null, Id = TnTComponentIdentifier.NewId() });
        StateHasChanged();

        return Task.CompletedTask;
    }

    private struct ToastMetadata {
        public required DateTimeOffset CreatedTime { get; set; }
        public required Task? Task { get; set; }
        public required string Id { get; set; }
    }
}