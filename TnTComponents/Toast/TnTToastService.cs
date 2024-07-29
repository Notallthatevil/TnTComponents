using Microsoft.JSInterop;
using TnTComponents.Core;

namespace TnTComponents.Toast;

internal class TnTToastService : ITnTToastService {

    public event ITnTToastService.OnCloseCallback? OnClose;

    public event ITnTToastService.OnOpenCallback? OnOpen;

    internal DotNetObjectReference<TnTToastService> Reference { get; private set; }

    public TnTToastService() {
        Reference = DotNetObjectReference.Create(this);
    }

    public async Task CloseAsync(ITnTToast snackbar) => await (OnClose?.Invoke(snackbar) ?? Task.CompletedTask);

    public async Task ShowAsync(string title, string? message = null, int timeout = 10, bool showClose = true, TnTColor backgroundColor = TnTColor.SurfaceVariant, TnTColor textColor = TnTColor.OnSurfaceVariant, TnTBorderRadius? borderRadius = null, int elevation = 2) =>
            await (OnOpen?.Invoke(new TnTToastImplementation() {
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

    public async Task ShowErrorAsync(string title, Exception? message, int timeout = 10, bool showClose = true, TnTBorderRadius? borderRadius = null, int elevation = 2) =>
        await ShowErrorAsync(title, message?.Message, timeout, showClose, borderRadius, elevation);

    public async Task ShowInfoAsync(string title, string? message = null, int timeout = 10, bool showClose = true, TnTBorderRadius? borderRadius = null, int elevation = 2) =>
        await ShowAsync(title, message, timeout, showClose, TnTColor.InfoContainer, TnTColor.Info, borderRadius, elevation);

    public async Task ShowSuccessAsync(string title, string? message = null, int timeout = 10, bool showClose = true, TnTBorderRadius? borderRadius = null, int elevation = 2) =>
        await ShowAsync(title, message, timeout, showClose, TnTColor.SuccessContainer, TnTColor.Success, borderRadius, elevation);

    public async Task ShowWarningAsync(string title, string? message = null, int timeout = 10, bool showClose = true, TnTBorderRadius? borderRadius = null, int elevation = 2) =>
        await ShowAsync(title, message, timeout, showClose, TnTColor.WarningContainer, TnTColor.Warning, borderRadius, elevation);

    internal class TnTToastImplementation : ITnTToast {
        public TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceVariant;
        public TnTBorderRadius? BorderRadius { get; set; } = new(2);
        public int Elevation { get; set; } = 2;
        public string? Message { get; set; }
        public bool ShowClose { get; set; } = true;
        public TnTColor TextColor { get; set; } = TnTColor.OnSurfaceVariant;
        public double Timeout { get; set; } = 10;
        public string Title { get; set; } = default!;
        public TextAlign? TextAlignment { get; }
        public bool Closing { get; internal set; }
    }
}