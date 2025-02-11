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
public class TnTToast : ComponentBase, IDisposable {

    /// <summary>
    ///     Gets or sets the toast service used to manage toasts.
    /// </summary>
    [Inject]
    private ITnTToastService _service { get; set; } = default!;

    private readonly ConcurrentDictionary<ITnTToast, ToastMetadata> _toasts = new();

    private readonly CancellationTokenSource _tokenSource = new();

    private const int CloseDelay = 250;

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
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        if (_toasts.Any()) {
            builder.OpenElement(0, "div");
            builder.AddAttribute(10, "class", CssClassBuilder.Create().AddClass("tnt-toast-container").Build());

            foreach (var (toast, metadata) in _toasts.OrderBy(kv => kv.Value.CreatedTime).Take(5).ToArray()) {
                if (metadata.Task is null) {
                    _toasts[toast] = metadata with {
                        Task = Task.Run(async () => {
                            await Task.Delay((int)TimeSpan.FromSeconds(toast.Timeout).TotalMilliseconds);
                            await InvokeAsync(() => OnClose(toast));
                        })
                    };
                }

                builder.OpenElement(20, "div");
                builder.AddAttribute(30, "class", CssClassBuilder.Create()
                    .AddClass("tnt-toast")
                    .AddTnTStyleable(toast)
                    .AddFilled()
                    .AddClass("tnt-closing", toast.Closing)
                    .Build());

                if (toast.Timeout > 0) {
                    builder.AddAttribute(40, "style", $"--timeout: {toast.Timeout}s;");
                }
                builder.SetKey(toast);

                {
                    builder.OpenElement(50, "div");
                    builder.AddAttribute(60, "class", "tnt-toast-header");

                    {
                        builder.OpenElement(70, "h3");
                        builder.AddContent(80, toast.Title);
                        builder.CloseElement();
                    }

                    if (toast.ShowClose) {
                        builder.OpenComponent<TnTImageButton>(90);
                        builder.AddComponentParameter(100, nameof(TnTImageButton.Icon), new MaterialIcon { Icon = MaterialIcon.Close });
                        builder.AddComponentParameter(110, nameof(TnTImageButton.OnClickCallback), EventCallback.Factory.Create<MouseEventArgs>(this, _ => _service.CloseAsync(toast)));
                        builder.AddComponentParameter(120, nameof(TnTImageButton.BackgroundColor), TnTColor.Transparent);
                        builder.AddComponentParameter(130, nameof(TnTImageButton.TextColor), TnTColor.Outline);
                        builder.AddComponentParameter(140, nameof(TnTImageButton.Elevation), 0);
                        builder.CloseComponent();
                    }

                    builder.CloseElement();
                }
                if (toast.Message is not null) {
                    builder.OpenElement(150, "div");
                    builder.AddAttribute(160, "class", "tnt-toast-body");
                    builder.AddContent(170, toast.Message);
                    builder.CloseElement();
                }

                {
                    if (toast.Timeout > 0) {
                        builder.OpenElement(180, "div");
                        builder.AddAttribute(190, "class", "tnt-toast-progress");
                        builder.AddAttribute(200, "style", $"background-color:var(--tnt-color-{toast.TextColor.ToCssClassName()})");
                        builder.CloseElement();
                    }
                }

                builder.CloseElement();
            }

            builder.CloseElement();
        }
    }

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
        await Task.Delay(CloseDelay);

        _toasts.Remove(toast, out _);

        StateHasChanged();
    }

    /// <summary>
    ///     Handles the open event for a toast.
    /// </summary>
    /// <param name="toast">The toast to open.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private Task OnOpen(ITnTToast toast) {
        _toasts.TryAdd(toast, new ToastMetadata() { CreatedTime = DateTimeOffset.Now, Task = null });
        StateHasChanged();

        return Task.CompletedTask;
    }

    private struct ToastMetadata {
        public required DateTimeOffset CreatedTime { get; set; }
        public required Task? Task { get; set; }
    }
}