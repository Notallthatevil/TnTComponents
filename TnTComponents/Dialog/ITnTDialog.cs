using System.Diagnostics.CodeAnalysis;

namespace TnTComponents.Dialog;

/// <summary>
///     Represents a dialog interface with various properties and methods for managing dialog behavior.
/// </summary>
public interface ITnTDialog {

    /// <summary>
    ///     Gets or sets the result of the dialog.
    /// </summary>
    DialogResult DialogResult { get; set; }

    /// <summary>
    ///     Gets the element ID of the dialog.
    /// </summary>
    string ElementId { get; init; }

    /// <summary>
    ///     Gets the options for configuring the dialog.
    /// </summary>
    TnTDialogOptions Options { get; init; }

    /// <summary>
    ///     Gets the parameters passed to the dialog.
    /// </summary>
    IReadOnlyDictionary<string, object?>? Parameters { get; init; }

    /// <summary>
    ///     Gets the type of the dialog.
    /// </summary>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
    Type Type { get; init; }

    /// <summary>
    ///     Asynchronously closes the dialog.
    /// </summary>
    /// <returns>A task that represents the asynchronous close operation.</returns>
    Task CloseAsync();
}