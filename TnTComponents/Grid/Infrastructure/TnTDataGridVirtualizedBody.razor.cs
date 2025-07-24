using Microsoft.AspNetCore.Components;
using TnTComponents.Virtualization;

namespace TnTComponents.Grid.Infrastructure;
[CascadingTypeParameter(nameof(TGridItem))]
public partial class TnTDataGridVirtualizedBody<TGridItem> {
    private int _delay;

    private TnTVirtualize<(int, TGridItem)> _virtualize = default!;

    private async ValueTask<TnTItemsProviderResult<(int, TGridItem)>> ProvideVirtualizedItemsAsync(TnTVirtualizeItemsProviderRequest<(int, TGridItem)> request) {

        // Debounce the requests. This eliminates a lot of redundant queries at the cost of slight lag after interactions.
        await Task.Delay(_delay);
        if (_delay < 2000) {
            Interlocked.Add(ref _delay, 100);
        }
        var result = default(TnTItemsProviderResult<(int, TGridItem)>);
        if (!request.CancellationToken.IsCancellationRequested) {
            // Combine the query parameters from Virtualize with the ones from PaginationState
            var startIndex = request.StartIndex;
            var count = request.Count;


            TnTGridItemsProviderRequest<TGridItem> providerRequest = new(startIndex, count, Context.SortBy, request.CancellationToken);
            var providerResult = await Context.Grid.ResolveItemsRequestAsync(providerRequest);

            if (!request.CancellationToken.IsCancellationRequested) {
                Context.TotalRowCount = providerResult.TotalItemCount;

                result = new TnTItemsProviderResult<(int, TGridItem)> {
                    Items = [.. providerResult.Items.Select((x, i) => ValueTuple.Create(i + request.StartIndex + 2, x))],
                    TotalItemCount = Context.TotalRowCount
                };
                Interlocked.Exchange(ref _delay, 0); // Reset the debounce delay
            }
        }

        return result;
    }

    public override async Task RefreshAsync() {
        await base.RefreshAsync();
        await InvokeAsync(async () => await (_virtualize?.RefreshDataAsync() ?? Task.CompletedTask));
    }
}
