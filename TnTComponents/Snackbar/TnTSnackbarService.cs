using Microsoft.JSInterop;
using TnTComponents.Core;

namespace TnTComponents.Snackbar;

public class TnTSnackbarService {

    public event OnCloseCallback? OnClose;

    public delegate Task OnCloseCallback(ITnTSnackbar snackbar);

    public event OnOpenCallback? OnOpen;

    public delegate Task OnOpenCallback(ITnTSnackbar snackbar);

    internal DotNetObjectReference<TnTSnackbarService> Reference { get; private set; }

    public TnTSnackbarService() {
        Reference = DotNetObjectReference.Create(this);
    }

    public async Task ShowAsync(string title, string? message = null, int timeout = 10, bool showClose = true, TnTColor backgroundColor = TnTColor.SurfaceVariant, TnTColor textColor = TnTColor.OnSurfaceVariant, TnTBorderRadius? borderRadius = null, int elevation = 2) =>
        await (OnOpen?.Invoke(new SnackbarImpl() {
            Timeout = timeout,
            ShowClose = showClose,
            Title = title,
            Message = message,
            BackgroundColor = backgroundColor,
            TextColor = textColor,
            BorderRadius = borderRadius,
            Elevation = elevation
        }) ?? Task.CompletedTask);

    public async Task ShowErrorAsync(string title, string? message = null, int timeout = 10, bool showClose = true, TnTBorderRadius? borderRadius = null, int elevation = 2) =>
        await ShowAsync(title, message, timeout, showClose, TnTColor.ErrorContainer, TnTColor.Error, borderRadius, elevation);

    public async Task ShowInfoAsync(string title, string? message = null, int timeout = 10, bool showClose = true, TnTBorderRadius? borderRadius = null, int elevation = 2) =>
        await ShowAsync(title, message, timeout, showClose, TnTColor.InfoContainer, TnTColor.Info, borderRadius, elevation);

    public async Task ShowSuccessAsync(string title, string? message = null, int timeout = 10, bool showClose = true, TnTBorderRadius? borderRadius = null, int elevation = 2) =>
        await ShowAsync(title, message, timeout, showClose, TnTColor.SuccessContainer, TnTColor.Success, borderRadius, elevation);

    public async Task ShowWarningAsync(string title, string? message = null, int timeout = 10, bool showClose = true, TnTBorderRadius? borderRadius = null, int elevation = 2) =>
        await ShowAsync(title, message, timeout, showClose, TnTColor.WarningContainer, TnTColor.Warning, borderRadius, elevation);

    internal async Task CloseAsync(ITnTSnackbar snackbar) => await (OnClose?.Invoke(snackbar) ?? Task.CompletedTask);

    private class SnackbarImpl : ITnTSnackbar {
        public TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceVariant;
        public TnTBorderRadius? BorderRadius { get; set; } = new(2);
        public int Elevation { get; set; } = 2;
        public string? Message { get; set; }
        public bool ShowClose { get; set; } = true;
        public TnTColor TextColor { get; set; } = TnTColor.OnSurfaceVariant;
        public int Timeout { get; set; } = 10;
        public string Title { get; set; } = default!;
    }
}