namespace TnTComponents.Virtualization;

/// <inheritdoc />
internal readonly record struct TnTItemsProviderRequest() : ITnTItemsProviderRequest {
    /// <inheritdoc />

    public int StartIndex { get; init; }

    /// <inheritdoc />
    public IEnumerable<KeyValuePair<string, SortDirection>> SortOnProperties { get; init; } = [];
    
    /// <inheritdoc />
    public int? Count { get; init; }
}