using Microsoft.AspNetCore.Components.QuickGrid;

namespace TnTComponents.Virtualization;

public interface ITnTVirtualizeItemsProviderRequest {
    int? Count { get; }
    IReadOnlyCollection<KeyValuePair<string, SortDirection>> SortOnProperties { get; }
    int StartIndex { get; }
}