using System.Diagnostics.CodeAnalysis;

namespace TnTComponents.Storage;

/// <summary>
///     Provides data for the change event.
/// </summary>
[ExcludeFromCodeCoverage]
public readonly struct ChangedEventArgs {

    /// <summary>
    ///     Gets the key of the changed item.
    /// </summary>
    public string Key { get; init; }

    /// <summary>
    ///     Gets the new value of the changed item.
    /// </summary>
    public object? NewValue { get; init; }

    /// <summary>
    ///     Gets the old value of the changed item.
    /// </summary>
    public object? OldValue { get; init; }
}