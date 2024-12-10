using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using TnTComponents.Core;
using TnTComponents.Virtualization;

namespace TnTComponents;

/// <summary>
/// A component that provides virtualization for a list of items.
/// </summary>
/// <typeparam name="TItem">The type of the items to be virtualized.</typeparam>
public partial class TnTVirtualize<TItem> {

    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-virtualize-container")
        .Build();

    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    /// <summary>
    /// Gets or sets a value indicating whether infinite scroll is enabled.
    /// </summary>
    [Parameter]
    public bool InfiniteScroll { get; set; } = true;

    /// <summary>
    /// Gets or sets the items provider.
    /// </summary>
    [Parameter, EditorRequired]
    public TnTVirtualizeItemsProvider<TItem>? ItemsProvider { get; set; }

    /// <summary>
    /// Gets or sets the template for rendering each item.
    /// </summary>
    [Parameter]
    public RenderFragment<TItem>? ItemTemplate { get; set; }

    public override string? JsModulePath => "./_content/TnTComponents/Virtualization/TnTVirtualize.razor.js";

    /// <summary>
    /// Gets or sets the template for rendering the loading indicator.
    /// </summary>
    [Parameter]
    public RenderFragment? LoadingTemplate { get; set; }

    /// <summary>
    /// Gets or sets the template for rendering when there are no items.
    /// </summary>
    [Parameter]
    public RenderFragment? EmptyTemplate { get; set; }

    /// <summary>
    /// Gets or sets the property to sort on.
    /// </summary>
    [Parameter]
    public Expression<Func<TItem, object>>? SortOnProperty { get; set; }

    private bool _allItemsRetrieved;
    private IEnumerable<TItem> _items = [];
    private TnTVirtualizeItemsProvider<TItem>? _lastUsedProvider;
    private CancellationTokenSource? _loadItemsCts;
    private KeyValuePair<string, SortDirection>? _sortOnProperty;
    private bool _loading;
    /// <summary>
    /// Initializes a new instance of the <see cref="TnTVirtualize{TItem}"/> class.
    /// </summary>
    [DynamicDependency(nameof(LoadMoreItems))]
    public TnTVirtualize() { }

    /// <summary>
    /// Loads more items asynchronously.
    /// </summary>
    [JSInvokable]
    public async Task LoadMoreItems() {
        if (_loadItemsCts is not null || ItemsProvider is null) {
            return;
        }
        _loading = true;
        _loadItemsCts = new CancellationTokenSource();
        var token = _loadItemsCts.Token;
        StateHasChanged(); // Allow the UI to display the loading indicator
        try {
            var result = await ItemsProvider(new TnTVirtualizeItemsProviderRequest<TItem> {
                StartIndex = _items.Count(),
                SortOnProperties = _sortOnProperty.HasValue ? [_sortOnProperty.Value] : [],
                CancellationToken = token
            });
            if (!token.IsCancellationRequested) {
                _items = _items.Concat(result.Items);

                if (_items.Count() == result.TotalItemCount) {
                    _allItemsRetrieved = true;
                }
                else if (IsolatedJsModule is not null) {
                    await IsolatedJsModule.InvokeVoidAsync("onNewItems", token, Element);
                }
            }
        }
        catch (OperationCanceledException oce) when (oce.CancellationToken == _loadItemsCts.Token) {
            // No-op; we canceled the operation, so it's fine to suppress this exception.
        }
        _loading = false;
        DisposeCancellationToken();
        StateHasChanged(); // Display the new items and hide the loading indicator
    }

    /// <summary>
    /// Refreshes the data asynchronously.
    /// </summary>
    public async Task RefreshDataAsync() {
        Reset();
        await LoadMoreItems();
    }

    protected override void Dispose(bool disposing) {
        if (disposing) {
            DisposeCancellationToken();
        }
        base.Dispose(disposing);
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
            if (SortOnProperty.Body is UnaryExpression unaryExpression) {
                if (unaryExpression.Operand is MemberExpression memberExpression) {
                    _sortOnProperty = new KeyValuePair<string, SortDirection>(memberExpression.Member.Name, SortDirection.Ascending);
                }
            }
            else if (SortOnProperty.Body is MemberExpression memberExpression) {
                _sortOnProperty = new KeyValuePair<string, SortDirection>(memberExpression.Member.Name, SortDirection.Ascending);
            }
        }
    }

    /// <summary>
    /// Disposes the cancellation token.
    /// </summary>
    private void DisposeCancellationToken() {
        try {
            _loadItemsCts?.Cancel();
            _loadItemsCts?.Dispose();
        }
        catch (ObjectDisposedException) { }
        finally {
            _loadItemsCts = null;
        }
    }

    /// <summary>
    /// Resets the component state.
    /// </summary>
    private void Reset() {
        _items = [];
        _allItemsRetrieved = false;
        _loading = false;
        DisposeCancellationToken();
    }
}

/// <summary>
/// Represents a request for items in a virtualized list.
/// </summary>
/// <typeparam name="TItem">The type of the items being requested.</typeparam>
public struct TnTVirtualizeItemsProviderRequest<TItem>() {
    /// <summary>
    /// Gets or sets the cancellation token for the request.
    /// </summary>
    public CancellationToken CancellationToken { get; init; }
    /// <summary>
    /// Gets or sets the maximum number of items to retrieve.
    /// </summary>
    public int? Count { get; set; }
    /// <summary>
    /// Gets or sets the properties to sort on and their sort directions.
    /// </summary>
    public IReadOnlyCollection<KeyValuePair<string, SortDirection>> SortOnProperties { get; init; } = [];
    /// <summary>
    /// Gets or sets the start index of the requested items.
    /// </summary>
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
