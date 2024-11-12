namespace TnTComponents.Virtualization;

/// <summary> Represents an item request. This struct is http query parameter binding friendly.
///
/// </summary> <remarks> To pass this object as a query parameter, use the following format: <code>
/// app.MapGet("/endpoint", (TnTItemsProviderRequest request) => ...); </code> This endpoint accepts
/// the following query parameters:
/// https://example.com?StartIndex=0&SortOnProperties=%5BPropertyName%2CAscending%5D%2C%5Bb%2C%20Descending%5D&Count=10 
/// Decoded = https://example.com?StartIndex=0&SortOnProperties=[PropertyName,Ascending],[PropertyName2,Descending]&Count=10 
/// </remarks>
public readonly record struct TnTItemsProviderRequest() {
    /// <summary>
    /// Gets or sets the start index of the requested items.
    /// </summary>
    public required readonly int StartIndex { get; init; }

    /// <summary>
    /// Gets or sets the properties to sort on and their sort directions.
    /// </summary>
    public required readonly IEnumerable<KeyValuePair<string, SortDirection>> SortOnProperties { get; init;} = [];
    /// <summary>
    /// Gets or sets the maximum number of items to retrieve.
    /// </summary>
    public required readonly int? Count { get; init; }
}
