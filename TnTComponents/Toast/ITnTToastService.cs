using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Core;
using TnTComponents.Toast;

namespace TnTComponents;

/// <summary>
///     Interface for the TnT Toast Service, providing methods to show and close toast notifications.
/// </summary>
public interface ITnTToastService {

    /// <summary>
    ///     Event triggered when a toast is closed.
    /// </summary>
    public event OnCloseCallback? OnClose;

    /// <summary>
    ///     Delegate for the OnClose event.
    /// </summary>
    /// <param name="snackbar">The toast that was closed.</param>
    public delegate Task OnCloseCallback(ITnTToast snackbar);

    /// <summary>
    ///     Event triggered when a toast is opened.
    /// </summary>
    public event OnOpenCallback? OnOpen;

    /// <summary>
    ///     Delegate for the OnOpen event.
    /// </summary>
    /// <param name="snackbar">The toast that was opened.</param>
    public delegate Task OnOpenCallback(ITnTToast snackbar);

    /// <summary>
    ///     Closes the specified toast asynchronously.
    /// </summary>
    /// <param name="snackbar">The toast to close.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task CloseAsync(ITnTToast snackbar);

    /// <summary>
    ///     Shows a toast with the specified parameters asynchronously.
    /// </summary>
    /// <param name="title">          The title of the toast.</param>
    /// <param name="message">        The message of the toast.</param>
    /// <param name="timeout">        
    ///     The timeout duration in seconds before the toast automatically closes.
    /// </param>
    /// <param name="showClose">      Indicates whether the close button should be shown.</param>
    /// <param name="backgroundColor">The background color of the toast.</param>
    /// <param name="textColor">      The text color of the toast.</param>
    /// <param name="borderRadius">   The border radius of the toast.</param>
    /// <param name="elevation">      The elevation level of the toast.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task ShowAsync(string title, string? message = null, int timeout = 10, bool showClose = true, TnTColor backgroundColor = TnTColor.SurfaceVariant, TnTColor textColor = TnTColor.OnSurfaceVariant, TnTBorderRadius? borderRadius = null, int elevation = 2);

    /// <summary>
    ///     Shows an error toast with the specified parameters asynchronously.
    /// </summary>
    /// <param name="title">       The title of the toast.</param>
    /// <param name="message">     The message of the toast.</param>
    /// <param name="timeout">     
    ///     The timeout duration in seconds before the toast automatically closes.
    /// </param>
    /// <param name="showClose">   Indicates whether the close button should be shown.</param>
    /// <param name="borderRadius">The border radius of the toast.</param>
    /// <param name="elevation">   The elevation level of the toast.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task ShowErrorAsync(string title, string? message = null, int timeout = 10, bool showClose = true, TnTBorderRadius? borderRadius = null, int elevation = 2);

    /// <summary>
    ///     Shows an error toast with the specified parameters asynchronously.
    /// </summary>
    /// <param name="title">       The title of the toast.</param>
    /// <param name="message">     The exception message to be displayed in the toast.</param>
    /// <param name="timeout">     
    ///     The timeout duration in seconds before the toast automatically closes.
    /// </param>
    /// <param name="showClose">   Indicates whether the close button should be shown.</param>
    /// <param name="borderRadius">The border radius of the toast.</param>
    /// <param name="elevation">   The elevation level of the toast.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task ShowErrorAsync(string title, Exception? message, int timeout = 10, bool showClose = true, TnTBorderRadius? borderRadius = null, int elevation = 2);

    /// <summary>
    ///     Shows an informational toast with the specified parameters asynchronously.
    /// </summary>
    /// <param name="title">       The title of the toast.</param>
    /// <param name="message">     The message of the toast.</param>
    /// <param name="timeout">     
    ///     The timeout duration in seconds before the toast automatically closes.
    /// </param>
    /// <param name="showClose">   Indicates whether the close button should be shown.</param>
    /// <param name="borderRadius">The border radius of the toast.</param>
    /// <param name="elevation">   The elevation level of the toast.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task ShowInfoAsync(string title, string? message = null, int timeout = 10, bool showClose = true, TnTBorderRadius? borderRadius = null, int elevation = 2);

    /// <summary>
    ///     Shows a success toast with the specified parameters asynchronously.
    /// </summary>
    /// <param name="title">       The title of the toast.</param>
    /// <param name="message">     The message of the toast.</param>
    /// <param name="timeout">     
    ///     The timeout duration in seconds before the toast automatically closes.
    /// </param>
    /// <param name="showClose">   Indicates whether the close button should be shown.</param>
    /// <param name="borderRadius">The border radius of the toast.</param>
    /// <param name="elevation">   The elevation level of the toast.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task ShowSuccessAsync(string title, string? message = null, int timeout = 10, bool showClose = true, TnTBorderRadius? borderRadius = null, int elevation = 2);

    /// <summary>
    ///     Shows a warning toast with the specified parameters asynchronously.
    /// </summary>
    /// <param name="title">       The title of the toast.</param>
    /// <param name="message">     The message of the toast.</param>
    /// <param name="timeout">     
    ///     The timeout duration in seconds before the toast automatically closes.
    /// </param>
    /// <param name="showClose">   Indicates whether the close button should be shown.</param>
    /// <param name="borderRadius">The border radius of the toast.</param>
    /// <param name="elevation">   The elevation level of the toast.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task ShowWarningAsync(string title, string? message = null, int timeout = 10, bool showClose = true, TnTBorderRadius? borderRadius = null, int elevation = 2);
}