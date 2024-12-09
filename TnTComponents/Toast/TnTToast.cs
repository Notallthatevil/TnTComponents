using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Collections.Concurrent;
using TnTComponents.Core;
using TnTComponents.Toast;
using static TnTComponents.Toast.TnTToastService;

namespace TnTComponents;

public class TnTToast : ComponentBase, IDisposable {

    [Inject]
    private ITnTToastService _service { get; set; } = default!;

    private readonly ConcurrentDictionary<ITnTToast, TimeOnly> _toasts = [];

    private ElementReference _element;
    private Func<Task>? _incrementAction;

    private readonly CancellationTokenSource _tokenSource = new();

    public void Dispose() {
        _tokenSource.Cancel();
        _tokenSource.Dispose();
        _service.OnClose -= OnClose;
        _service.OnOpen -= OnOpen;
        GC.SuppressFinalize(this);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        if (_toasts.Any()) {
            builder.OpenElement(0, "div");
            builder.AddAttribute(10, "class", CssClassBuilder.Create().AddClass("tnt-toast-container").Build());

            foreach (var pair in _toasts.OrderBy(kv => kv.Value).Take(5)) {
                var toast = pair.Key;
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

                        var startTime = pair.Value;
                        var endTime = pair.Value.Add(new TimeSpan(0, 0, (int)(toast.Timeout)));

                        Task.Run(async () => {
                            await Task.Delay((int)toast.Timeout * 1000);
                            await _service.CloseAsync(toast);
                        }, _tokenSource.Token);

                        builder.CloseElement();
                    }
                }

                builder.CloseElement();
            }

            builder.AddElementReferenceCapture(300, e => _element = e);

            builder.CloseElement();
        }
    }

    protected override void OnInitialized() {
        base.OnInitialized();
        _service.OnOpen += OnOpen;
        _service.OnClose += OnClose;
    }

    private async Task OnClose(ITnTToast toast) {
        var impl = toast as TnTToastImplementation;
        impl!.Closing = true;
        StateHasChanged();
        await Task.Delay(250);

        _toasts.Remove(toast, out var _);

        if (_toasts.IsEmpty) {
            _incrementAction = null;
        }

        StateHasChanged();
    }

    private Task OnOpen(ITnTToast toast) {
        _toasts.TryAdd(toast, TimeOnly.FromDateTime(DateTime.UtcNow));
        StateHasChanged();

        if (_incrementAction is null) {
            _incrementAction = async () => {
                while (_toasts.Count != 0) {
                    await Task.Delay(100);
                    StateHasChanged();
                }
            };

            _incrementAction.Invoke();
        }
        return Task.CompletedTask;
    }
}
