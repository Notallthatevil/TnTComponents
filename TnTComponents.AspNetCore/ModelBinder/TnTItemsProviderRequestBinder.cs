using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TnTComponents.AspNetCore.ModelBinder;
public class TnTItemsProviderRequestBinder : IModelBinder {
    private readonly record struct BindableTnTItemsProviderRequest() : ITnTItemsProviderRequest {
        public required int StartIndex { get; init; }
        public required IEnumerable<KeyValuePair<string, SortDirection>> SortOnProperties { get; init; } = [];
        public required int? Count { get; init; }
    }

    public Task BindModelAsync(ModelBindingContext bindingContext) {
        ArgumentNullException.ThrowIfNull(bindingContext, nameof(bindingContext));

        if (bindingContext.BindingSource == BindingSource.Query) {
            var query = bindingContext.HttpContext?.Request?.Query;
            if (query is not null && query.TryGetValue(nameof(ITnTItemsProviderRequest.StartIndex), out var startIndexes) && !string.IsNullOrWhiteSpace(startIndexes.FirstOrDefault()) && int.TryParse(startIndexes.FirstOrDefault(), out var startIndex)) {
                int? count = null;
                var countStr = query[nameof(ITnTItemsProviderRequest.Count)];
                if (!string.IsNullOrWhiteSpace(countStr.FirstOrDefault()) && int.TryParse(countStr.FirstOrDefault(), out var countResult)) {
                    count = countResult;
                }
                var sortOnProperties = query[nameof(ITnTItemsProviderRequest.SortOnProperties)].SelectMany(s => s!.Split("],[").Select(t => t.Replace("[", string.Empty).Replace("]", string.Empty)))
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(s => {
                        var split = s.Split(',');
                        return new KeyValuePair<string, SortDirection>(split.First(), Enum.Parse<SortDirection>(split.LastOrDefault() ?? SortDirection.Auto.ToString()));
                    })
                    .ToList();

                ITnTItemsProviderRequest result = new BindableTnTItemsProviderRequest {
                    StartIndex = startIndex,
                    Count = count,
                    SortOnProperties = sortOnProperties
                };

                bindingContext.Result = ModelBindingResult.Success(result);
            }
            else {
                bindingContext.Result = ModelBindingResult.Failed();
            }
        }
        return Task.CompletedTask;
    }
}
