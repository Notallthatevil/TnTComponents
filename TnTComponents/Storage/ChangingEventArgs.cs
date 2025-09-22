using System.Diagnostics.CodeAnalysis;

namespace TnTComponents.Storage;

/// <summary>
///     Provides data for the changing event.
/// </summary>
[ExcludeFromCodeCoverage]
public class ChangingEventArgs {

    /// <summary>
    ///     Gets a value indicating whether the event should be canceled.
    /// </summary>
    public bool Cancel { get; init; }

    /// <summary>
    ///     Gets the key associated with the change.
    /// </summary>
    public required string Key { get; init; }

    /// <summary>
    ///     Gets the new value associated with the change.
    /// </summary>
    public object? NewValue { get; init; }

    /// <summary>
    ///     Gets the old value associated with the change.
    /// </summary>
    public object? OldValue { get; init; }
}