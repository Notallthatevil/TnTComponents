using TnTComponents.Core;
using TnTComponents.Interfaces;

namespace TnTComponents.Toast;

/// <summary>
///     Represents a toast notification component with customizable properties.
/// </summary>
public interface ITnTToast : ITnTStyleable {

    /// <summary>
    ///     Gets a value indicating whether the toast is in the process of closing.
    /// </summary>
    bool Closing { get; }

    /// <summary>
    ///     Gets or sets the message to be displayed in the toast.
    /// </summary>
    string? Message { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the close button should be shown.
    /// </summary>
    bool ShowClose { get; set; }

    /// <summary>
    ///     Gets or sets the timeout duration in milliseconds before the toast automatically closes.
    /// </summary>
    double Timeout { get; set; }

    /// <summary>
    ///     Gets or sets the title of the toast.
    /// </summary>
    string Title { get; set; }
}