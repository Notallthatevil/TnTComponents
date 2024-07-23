using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System.Collections.Concurrent;
using TnTComponents.Snackbar;

namespace TnTComponents;

public class TnTSnackbar : ComponentBase, IDisposable {

    [Inject]
    private TnTSnackbarService _service { get; set; } = default!;

    private readonly ConcurrentDictionary<ITnTSnackbar, TimeOnly> _snackbars = [];

    private readonly ConcurrentQueue<ITnTSnackbar> _waitingSnackbars = [];

    private Func<Task>? _incrementAction = null;

    public void Dispose() {
        _service.OnClose -= OnClose;
        _service.OnOpen -= OnOpen;
        GC.SuppressFinalize(this);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        if (_snackbars.Any()) {
            builder.OpenElement(0, "div");
            builder.AddAttribute(10, "class", "tnt-components tnt-snackbar-container");

            foreach (var pair in _snackbars.OrderBy(kv => kv.Value)) {
                var snackbar = pair.Key;
                builder.OpenElement(20, "div");
                builder.AddAttribute(30, "class", snackbar.GetClass());
                if (snackbar.Timeout > 0) {
                    builder.AddAttribute(45, "style", $"--timeout: {snackbar.Timeout}s;");
                }
                builder.SetKey(snackbar);

                if (snackbar.ShowClose) {
                    builder.OpenComponent<TnTImageButton>(40);
                    builder.AddComponentParameter(50, nameof(TnTImageButton.Icon), new MaterialIcon(MaterialIcon.Close));
                    builder.AddComponentParameter(60, nameof(TnTImageButton.OnClickCallback), EventCallback.Factory.Create<MouseEventArgs>(this, async _ => await _service.CloseAsync(snackbar)));
                    builder.CloseComponent();
                }

                {
                    builder.OpenElement(70, "h4");
                    builder.AddContent(80, snackbar.Title);
                    builder.CloseComponent();
                }

                if (snackbar.Message is not null) {
                    builder.OpenElement(90, "div");
                    builder.AddAttribute(100, "class", "tnt-body-small");
                    builder.AddContent(110, snackbar.Message);
                    builder.CloseElement();
                }

                {
                    if (snackbar.Timeout > 0) {
                        builder.OpenElement(120, "div");
                        builder.AddAttribute(130, "style", $"background-color:var(--tnt-color-{snackbar.TextColor.ToCssClassName()})");

                        var startTime = pair.Value;
                        var endTime = pair.Value.Add(new TimeSpan(0, 0, snackbar.Timeout + 1));
                        var currentTime = TimeOnly.FromDateTime(DateTime.UtcNow);
                        var elapsedTime = (currentTime - startTime).TotalMilliseconds;

                        if (currentTime >= endTime) {
                            _ = _service.CloseAsync(snackbar);
                        }

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

    private Task AddReadySnackbar(ITnTSnackbar snackbar) {
        _snackbars.TryAdd(snackbar, TimeOnly.FromDateTime(DateTime.UtcNow));
        StateHasChanged();

        if (_incrementAction is null) {
            _incrementAction = async () => {
                while (_snackbars.Count != 0) {
                    await Task.Delay(100);
                    StateHasChanged();
                }
            };

            _incrementAction.Invoke();
        }
        return Task.CompletedTask;
    }

    private Task OnClose(ITnTSnackbar snackbar) {
        _snackbars.Remove(snackbar, out var _);

        if (_waitingSnackbars.Count != 0) {
            if (_waitingSnackbars.TryDequeue(out var newSnackbar)) {
                AddReadySnackbar(newSnackbar);
                Console.WriteLine($"Popping from stack. Count {_waitingSnackbars.Count}");
            }
        }

        if (_snackbars.Count == 0) {
            _incrementAction = null;
        }

        StateHasChanged();
        return Task.CompletedTask;
    }

    private Task OnOpen(ITnTSnackbar snackbar) {
        if (_snackbars.Count >= 5) {
            _waitingSnackbars.Enqueue(snackbar);
            Console.WriteLine($"Pushing to stack. Count {_waitingSnackbars.Count}");
            return Task.CompletedTask;
        }
        else {
            return AddReadySnackbar(snackbar);
        }
    }
}