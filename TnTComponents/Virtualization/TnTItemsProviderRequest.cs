using Microsoft.AspNetCore.Http;

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
    public int StartIndex { get; init; }

    /// <summary>
    /// Gets or sets the properties to sort on and their sort directions.
    /// </summary>
    public IEnumerable<KeyValuePair<string, SortDirection>> SortOnProperties { get; init; } = [];
    /// <summary>
    /// Gets or sets the maximum number of items to retrieve.
    /// </summary>
    public int? Count { get; init; }

    /// <summary>
    /// Binds the request from the provided HTTP context.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>
    /// A <see cref="ValueTask{TResult}" /> representing the asynchronous operation, containing the
    /// bound <see cref="TnTItemsProviderRequest" /> or <c>null</c> if the binding failed.
    /// </returns>
    public static ValueTask<TnTItemsProviderRequest?> BindAsync(HttpContext context) {
        var query = context.Request.Query;
        if (query.TryGetValue(nameof(StartIndex), out var startIndexes) && !string.IsNullOrWhiteSpace(startIndexes.FirstOrDefault()) && int.TryParse(startIndexes.FirstOrDefault(), out int startIndex)) {
            int? count = null;
            var countStr = query[nameof(Count)];
            if (!string.IsNullOrWhiteSpace(countStr.FirstOrDefault()) && int.TryParse(countStr.FirstOrDefault(), out int countResult)) {
                count = countResult;
            }
            var sortOnProperties = query[nameof(SortOnProperties)].SelectMany(s => s.Split("],[").Select(t => t.Replace("[", string.Empty).Replace("]", string.Empty)))
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => {
                    var split = s.Split(',');
                    return new KeyValuePair<string, SortDirection>(split.First(), Enum.Parse<SortDirection>(split.LastOrDefault() ?? SortDirection.Auto.ToString()));
                })
                .ToList();

            return ValueTask.FromResult<TnTItemsProviderRequest?>(new TnTItemsProviderRequest {
                StartIndex = startIndex,
                Count = count,
                SortOnProperties = sortOnProperties
            });
        }

        return ValueTask.FromResult((TnTItemsProviderRequest?)null);
    }
}