using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Core;
using TnTComponents.Toast;

namespace TnTComponents;

public interface ITnTToastService {

    public event OnCloseCallback? OnClose;

    public delegate Task OnCloseCallback(ITnTToast snackbar);

    public event OnOpenCallback? OnOpen;

    public delegate Task OnOpenCallback(ITnTToast snackbar);

    Task CloseAsync(ITnTToast snackbar);

    Task ShowAsync(string title, string? message = null, int timeout = 10, bool showClose = true, TnTColor backgroundColor = TnTColor.SurfaceVariant, TnTColor textColor = TnTColor.OnSurfaceVariant, TnTBorderRadius? borderRadius = null, int elevation = 2);

    Task ShowErrorAsync(string title, string? message = null, int timeout = 10, bool showClose = true, TnTBorderRadius? borderRadius = null, int elevation = 2);

    Task ShowErrorAsync(string title, Exception? message, int timeout = 10, bool showClose = true, TnTBorderRadius? borderRadius = null, int elevation = 2);

    Task ShowInfoAsync(string title, string? message = null, int timeout = 10, bool showClose = true, TnTBorderRadius? borderRadius = null, int elevation = 2);

    Task ShowSuccessAsync(string title, string? message = null, int timeout = 10, bool showClose = true, TnTBorderRadius? borderRadius = null, int elevation = 2);

    Task ShowWarningAsync(string title, string? message = null, int timeout = 10, bool showClose = true, TnTBorderRadius? borderRadius = null, int elevation = 2);
}