using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using TnTComponents.Core;
using TnTComponents.Virtualization;

namespace TnTComponents;

public partial class TnTVirtualize<TItem> {

    public override string? CssClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-virtualize-container")
        .Build();

    public override string? CssStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    [Parameter]
    public bool InfiniteScroll { get; set; } = true;

    [Parameter, EditorRequired]
    public TnTVirtualizeItemsProvider<TItem>? ItemsProvider { get; set; }

    [Parameter]
    public RenderFragment<TItem>? ItemTemplate { get; set; }

    public override string? JsModulePath => "./_content/TnTComponents/Virtualization/TnTVirtualize.razor.js";

    [Parameter]
    public RenderFragment? LoadingTemplate { get; set; }

    [Parameter]
    public Expression<Func<TItem, object>>? SortOnProperty { get; set; }

    private bool _allItemsRetrieved;
    private IEnumerable<TItem> _items = [];
    private TnTVirtualizeItemsProvider<TItem>? _lastUsedProvider;
    private CancellationTokenSource? _loadItemsCts;
    private KeyValuePair<string, SortDirection>? _sortOnProperty;

    [DynamicDependency(nameof(LoadMoreItems))]
    public TnTVirtualize() { }

    public override async ValueTask DisposeAsync() {
        GC.SuppressFinalize(this);
        DisposeCancellationToken();
        await base.DisposeAsync();
    }

    [JSInvokable]
    public async Task LoadMoreItems() {
        if (_loadItemsCts is not null || ItemsProvider is null) {
            return;
        }

        _loadItemsCts = new CancellationTokenSource();
        StateHasChanged(); // Allow the UI to display the loading indicator
        try {
            var result = await ItemsProvider(new TnTVirtualizeItemsProviderRequest<TItem> {
                StartIndex = _items.Count(),
                SortOnProperties = _sortOnProperty.HasValue ? [_sortOnProperty.Value] : [],
                CancellationToken = _loadItemsCts.Token
            });
            if (!_loadItemsCts.IsCancellationRequested) {
                _items = _items.Concat(result.Items);

                if (_items.Count() == result.TotalItemCount) {
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
        if (!InfiniteScroll) {
            throw new NotImplementedException("Non-infinite scroll has not been implemented");
        }

        if (ItemsProvider != _lastUsedProvider) {
            Reset();
        }

        _lastUsedProvider = ItemsProvider;

        if (SortOnProperty is not null) {
            if(SortOnProperty.Body is UnaryExpression unaryExpression) {
                if (unaryExpression.Operand is MemberExpression memberExpression) {
                    _sortOnProperty = new KeyValuePair<string, SortDirection>(memberExpression.Member.Name, SortDirection.Ascending);
                }
            }
            else if (SortOnProperty.Body is MemberExpression memberExpression) {
                _sortOnProperty = new KeyValuePair<string, SortDirection>(memberExpression.Member.Name, SortDirection.Ascending);
            }
        }
    }

    private void DisposeCancellationToken() {
        _loadItemsCts?.Cancel();
        _loadItemsCts?.Dispose();
        _loadItemsCts = null;
    }

    private void Reset() {
        _items = [];
        _allItemsRetrieved = false;
        DisposeCancellationToken();
    }
}

public class TnTVirtualizeItemsProviderRequest<TItem> {
    public CancellationToken CancellationToken { get; init; }
    public int? Count { get; set; }
    public IReadOnlyCollection<KeyValuePair<string, SortDirection>> SortOnProperties { get; init; } = [];
    public int StartIndex { get; init; }

    public static implicit operator TnTItemsProviderRequest(TnTVirtualizeItemsProviderRequest<TItem> request) {
        return new TnTItemsProviderRequest {
            StartIndex = request.StartIndex,
            SortOnProperties = request.SortOnProperties,
            Count = request.Count
        };
    }

    public static implicit operator TnTVirtualizeItemsProviderRequest<TItem>(TnTItemsProviderRequest request) {
        return new TnTVirtualizeItemsProviderRequest<TItem> {
            Count = request.Count,
            SortOnProperties = request.SortOnProperties.ToList(),
            StartIndex = request.StartIndex,
            CancellationToken = default
        };
    }
}

/// <summary>
/// A callback that provides data for a <see cref="TnTDataGrid{TGridItem}" />.
/// </summary>
/// <typeparam name="TItem">The type of data represented by each row in the grid.</typeparam>
/// <param name="request">Parameters describing the data being requested.</param>
/// <returns>
/// A <see cref="ValueTask{TnTItemsProviderResult{TGridItem}}" /> that gives the data to be displayed.
/// </returns>
public delegate ValueTask<TnTItemsProviderResult<TItem>> TnTVirtualizeItemsProvider<TItem>(TnTVirtualizeItemsProviderRequest<TItem> request);