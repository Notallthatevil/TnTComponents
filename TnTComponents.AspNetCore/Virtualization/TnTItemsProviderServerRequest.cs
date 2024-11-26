using Microsoft.AspNetCore.Http;
using System.Drawing;
using TnTComponents.Virtualization;

namespace TnTComponents.AspNetCore.Virtualization;

public readonly record struct TnTItemsProviderServerRequest() {

    /// <summary>
    /// Gets or sets the start index of the requested items.
    /// </summary>
    public readonly int StartIndex { get; init; } = 0;

    /// <summary>
    /// Gets or sets the properties to sort on and their sort directions.
    /// </summary>
    public readonly IEnumerable<KeyValuePair<string, SortDirection>> SortOnProperties { get; init; } = [];
    /// <summary>
    /// Gets or sets the maximum number of items to retrieve.
    /// </summary>
    public readonly int? Count { get; init; }

    public static implicit operator TnTItemsProviderRequest(TnTItemsProviderServerRequest request) {
        return new TnTItemsProviderRequest {
            StartIndex = request.StartIndex,
            SortOnProperties = request.SortOnProperties,
            Count = request.Count
        };
    }

    public static implicit operator TnTItemsProviderServerRequest(TnTItemsProviderRequest request) {
        return new TnTItemsProviderServerRequest {
            Count = request.Count,
            SortOnProperties = request.SortOnProperties.ToList(),
            StartIndex = request.StartIndex
        };
    }

    public static ValueTask<TnTItemsProviderServerRequest?> BindAsync(HttpContext context) {
        var query = context.Request.Query;
        if (query.TryGetValue(nameof(StartIndex), out var startIndexes) && !string.IsNullOrWhiteSpace(startIndexes.FirstOrDefault()) && int.TryParse(startIndexes.FirstOrDefault(), out int startIndex)) {
            int? count = null;
            var countStr = query[nameof(Count)];
            if (!string.IsNullOrWhiteSpace(countStr.FirstOrDefault()) && int.TryParse(countStr.FirstOrDefault(), out int countResult)) {
                count = countResult;
            }
            var sortOnProperties = query[nameof(SortOnProperties)].SelectMany(s => s!.Split("],[").Select(t => t.Replace("[", string.Empty).Replace("]", string.Empty)))
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => {
                    var split = s.Split(',');
                    return new KeyValuePair<string, SortDirection>(split.First(), Enum.Parse<SortDirection>(split.LastOrDefault() ?? SortDirection.Auto.ToString()));
                })
                .ToList();

            return ValueTask.FromResult<TnTItemsProviderServerRequest?>(new TnTItemsProviderServerRequest {
                StartIndex = startIndex,
                Count = count,
                SortOnProperties = sortOnProperties
            });
        }

        return ValueTask.FromResult((TnTItemsProviderServerRequest?)null);
    }
}
