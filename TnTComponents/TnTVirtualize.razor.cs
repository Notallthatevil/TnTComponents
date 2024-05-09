using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTVirtualize<TItem> {

    public override string? CssClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-virtualize-container")
        .Build();

    public override string? CssStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    [Parameter, EditorRequired]
    public TnTVirtualizeItemsRequestDelegate<TItem> ItemsProvider { get; set; } = default!;

    [Parameter]
    public RenderFragment<TItem>? ItemTemplate { get; set; }

    [Parameter]
    public bool InfiniteScroll { get; set; } = true;

    public override string? JsModulePath => "./_content/TnTComponents/TnTVirtualize.razor.js";

    [Parameter]
    public RenderFragment? LoadingTemplate { get; set; }

    private bool _allItemsRetrieved;
    private ICollection<TItem> _items = [];
    private TnTVirtualizeItemsRequestDelegate<TItem> _lastUsedProvider = default!;
    private CancellationTokenSource? _loadItemsCts;

    public override async ValueTask DisposeAsync() {
        GC.SuppressFinalize(this);
        DisposeCancellationToken();
        await base.DisposeAsync();
    }

    [DynamicDependency(nameof(LoadMoreItems))]
    public TnTVirtualize() { }

    [JSInvokable]
    public async Task LoadMoreItems() {
        if (_loadItemsCts is not null) {
            return;
        }

        _loadItemsCts = new CancellationTokenSource();
        StateHasChanged(); // Allow the UI to display the loading indicator
        try {
            var result = await ItemsProvider(new TnTVirtualizeItemsRequest { StartIndex = _items.Count, CancellationToken = _loadItemsCts.Token });
            if (!_loadItemsCts.IsCancellationRequested) {
                var length = _items.Count;
                foreach (var item in result.Items) {
                    _items.Add(item);
                }

                if (_items.Count == result.TotalItemsCount) {
                    _allItemsRetrieved = true;
                }
                else if (IsolatedJsModule is not null) {
                    await IsolatedJsModule.InvokeVoidAsync("onNewItems", Element);
                }
            }
        }
        catch (OperationCanceledException oce) when (oce.CancellationToken == _loadItemsCts.Token) {
            // No-op; we canceled the operation, so it's fine to suppress this exception.
        }

        DisposeCancellationToken();

        StateHasChanged(); // Display the new items and hide the loading indicator
    }

    public async Task RefreshDataAsync() {
        Reset();
        await LoadMoreItems();
    }

    protected override void OnParametersSet() {
        base.OnParametersSet();
        if (ItemsProvider is null) {
            throw new InvalidOperationException($"{nameof(ItemsProvider)} is required.");
        }
        if (!InfiniteScroll) {
            throw new NotImplementedException("Non-infinite scroll has not been implemented");
        }

        if (ItemsProvider != _lastUsedProvider) {
            Reset();
        }

        _lastUsedProvider = ItemsProvider;
    }

    private void Reset() {
        _items = [];
        _allItemsRetrieved = false;
        DisposeCancellationToken();
    }

    private void DisposeCancellationToken() {
        _loadItemsCts?.Cancel();
        _loadItemsCts?.Dispose();
        _loadItemsCts = null;
    }
}

public class TnTVirtualizeItemsRequest {
    public CancellationToken CancellationToken { get; init; }
    public int? RequestedCount { get; init; }
    public int StartIndex { get; init; }
}

public sealed class TnTVirtualizeItemsResult<TItem> {
    public required IReadOnlyCollection<TItem> Items { get; init; }
    public required int TotalItemsCount { get; init; }
}

public delegate Task<TnTVirtualizeItemsResult<TItem>> TnTVirtualizeItemsRequestDelegate<TItem>(TnTVirtualizeItemsRequest request);