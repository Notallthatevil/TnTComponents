using Microsoft.AspNetCore.Components;
using TnTComponents.Virtualization;

namespace TnTComponents.Grid.Infrastructure;
[CascadingTypeParameter(nameof(TGridItem))]
public partial class TnTDataGridVirtualizedBody<TGridItem> {
    private const int _defaultDelay = 500;
    private int _delay = _defaultDelay;

    private TnTVirtualize<(int, TGridItem)> _virtualize = default!;

    private async ValueTask<TnTItemsProviderResult<(int, TGridItem)>> ProvideVirtualizedItemsAsync(TnTVirtualizeItemsProviderRequest<(int, TGridItem)> request) {

        // Debounce the requests. This eliminates a lot of redundant queries at the cost of slight lag after interactions.
        await Task.Delay(_delay);
        if (_delay < 2000) {
            Interlocked.Add(ref _delay, 250);
        }
        var result = default(TnTItemsProviderResult<(int, TGridItem)>);
        if (!request.CancellationToken.IsCancellationRequested) {

            var providerResult = await Context.Grid.ResolveItemsRequestAsync(new() {
                StartIndex = request.StartIndex,
                Count = request.Count,
                CancellationToken = request.CancellationToken,
                SortBy = Context.SortBy
            });

            if (!request.CancellationToken.IsCancellationRequested) {
                Context.TotalRowCount = providerResult.TotalItemCount;

                result = new TnTItemsProviderResult<(int, TGridItem)> {
                    Items = [.. providerResult.Items.Select((x, i) => ValueTuple.Create(i + request.StartIndex + 2, x))],
                    TotalItemCount = Context.TotalRowCount
                };
                Interlocked.Exchange(ref _delay, _defaultDelay); // Reset the debounce delay
            }
        }

        return result;
    }

    public override async Task RefreshAsync() {
        await base.RefreshAsync();
        await InvokeAsync(async () => await (_virtualize?.RefreshDataAsync() ?? Task.CompletedTask));
    }
}
