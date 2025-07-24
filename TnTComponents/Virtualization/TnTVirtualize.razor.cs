using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using TnTComponents.Core;
using TnTComponents.Virtualization;

namespace TnTComponents;

/// <summary>
///     A component that provides virtualization for a list of items.
/// </summary>
/// <typeparam name="TItem">The type of the items to be virtualized.</typeparam>
/// <remarks>Initializes a new instance of the <see cref="TnTVirtualize{TItem}" /> class.</remarks>
[method: DynamicDependency(nameof(LoadMoreItems))]
[CascadingTypeParameter("TGridItem")]
public partial class TnTVirtualize<TItem>() {

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-virtualize-container")
        .Build();

    /// <inheritdoc />
    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    /// <summary>
    ///     Gets or sets the template for rendering when there are no items.
    /// </summary>
    [Parameter]
    public RenderFragment? EmptyTemplate { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether infinite scroll is enabled.
    /// </summary>
    [Parameter]
    public bool InfiniteScroll { get; set; } = true;

    /// <summary>
    ///     Gets or sets the items provider.
    /// </summary>
    [Parameter, EditorRequired]
    public TnTVirtualizeItemsProvider<TItem>? ItemsProvider { get; set; }

    /// <summary>
    ///     Gets or sets the template for rendering each item.
    /// </summary>
    [Parameter]
    public RenderFragment<TItem>? ItemTemplate { get; set; }

    /// <inheritdoc />
    public override string? JsModulePath => "./_content/TnTComponents/Virtualization/TnTVirtualize.razor.js";

    /// <summary>
    ///     Indicates whether the component is currently loading items.
    /// </summary>
    public bool Loading { get; private set; } = true;

    /// <summary>
    ///     Gets or sets the template for rendering the loading indicator.
    /// </summary>
    [Parameter]
    public RenderFragment? LoadingTemplate { get; set; }

    /// <summary>
    ///     Gets or sets the property to sort on.
    /// </summary>
    [Parameter]
    public IEnumerable<SortedProperty>? Sort { get; set; }
    [Parameter]
    public int LoadCount { get; set; } = 15;

    private bool _allItemsRetrieved;
    private IEnumerable<TItem> _items = [];
    private TnTVirtualizeItemsProvider<TItem>? _lastUsedProvider;
    private CancellationTokenSource? _loadItemsCts;

    /// <summary>
    ///     Loads more items asynchronously.
    /// </summary>
    [JSInvokable]
    public async Task LoadMoreItems() {
        if (ItemsProvider is null) {
            return;
        }

        _loadItemsCts?.Cancel();
        _loadItemsCts?.Dispose();
        var scopeCts = new CancellationTokenSource();
        var token = scopeCts.Token;
        _loadItemsCts = scopeCts;
        try {
            Loading = true;

            StateHasChanged(); // Allow the UI to display the
                               // loading indicator
            var result = await ItemsProvider(new TnTVirtualizeItemsProviderRequest<TItem> {
                StartIndex = _items.Count(),
                SortOnProperties = Sort?.Select(s => new KeyValuePair<string, SortDirection>(s.PropertyName, s.Direction)).ToList() ?? [],
                CancellationToken = token,
                Count = LoadCount
            });
            if (!token.IsCancellationRequested) {
                if (_items.Count() > result.TotalItemCount) {
                    _items = result.Items;
                }
                else {
                    _items = _items.Concat(result.Items);
                }

                if (_items.Count() >= result.TotalItemCount) {
                    _allItemsRetrieved = true;
                }
                else if (IsolatedJsModule is not null) {
                    await IsolatedJsModule.InvokeVoidAsync("onNewItems", token, Element);
                }
            }
        }
        catch (OperationCanceledException oce) when (oce.CancellationToken == token) { }
        Loading = false;
        _loadItemsCts = null;
        StateHasChanged(); // Display the new items and hide the loading indicator
    }

    /// <summary>
    ///     Refreshes the data asynchronously.
    /// </summary>
    public async Task RefreshDataAsync() {
        Reset();
        if (IsolatedJsModule is not null) {
            await IsolatedJsModule.InvokeVoidAsync("resetScrollPosition", Element);
        }
        await LoadMoreItems();
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing) {
        if (disposing) {
            _loadItemsCts?.Cancel();
            _loadItemsCts?.Dispose();
            _loadItemsCts = null;
        }
        base.Dispose(disposing);
    }

    /// <inheritdoc />
    protected override void OnParametersSet() {
        base.OnParametersSet();
        if (!InfiniteScroll) {
            throw new NotImplementedException("Non-infinite scroll has not been implemented");
        }

        if (ItemsProvider != _lastUsedProvider) {
            Reset();
        }

        _lastUsedProvider = ItemsProvider;

    }

    /// <summary>
    ///     Resets the component state.
    /// </summary>
    public void Reset() {
        _items = [];
        _allItemsRetrieved = false;
        Loading = true;
        StateHasChanged();
    }
}

/// <summary>
///     Represents a request for items in a virtualized list.
/// </summary>
/// <typeparam name="TItem">The type of the items being requested.</typeparam>
public struct TnTVirtualizeItemsProviderRequest<TItem>() {

    /// <summary>
    ///     Gets or sets the cancellation token for the request.
    /// </summary>
    public CancellationToken CancellationToken { get; init; }

    /// <summary>
    ///     Gets or sets the maximum number of items to retrieve.
    /// </summary>
    public int? Count { get; set; }

    /// <summary>
    ///     Gets or sets the properties to sort on and their sort directions.
    /// </summary>
    public IReadOnlyCollection<KeyValuePair<string, SortDirection>> SortOnProperties { get; init; } = [];

    /// <summary>
    ///     Gets or sets the start index of the requested items.
    /// </summary>
    public int StartIndex { get; init; }

    /// <summary>
    ///     Implicitly converts a <see cref="TnTVirtualizeItemsProviderRequest{TItem}" /> to a <see cref="TnTItemsProviderRequest" />.
    /// </summary>
    /// <param name="request">The virtualize items provider request to convert.</param>
    /// <returns>A new <see cref="TnTItemsProviderRequest" /> with properties copied from the source request.</returns>
    /// <remarks>This conversion enables interoperability between the virtualization-specific request type and the general-purpose items provider request type.</remarks>
    public static implicit operator TnTItemsProviderRequest(TnTVirtualizeItemsProviderRequest<TItem> request) {
        return new TnTItemsProviderRequest {
            StartIndex = request.StartIndex,
            SortOnProperties = request.SortOnProperties,
            Count = request.Count
        };
    }

    /// <summary>
    ///     Implicitly converts a <see cref="TnTItemsProviderRequest" /> to a <see cref="TnTVirtualizeItemsProviderRequest{TItem}" />.
    /// </summary>
    /// <param name="request">The items provider request to convert.</param>
    /// <returns>A new <see cref="TnTVirtualizeItemsProviderRequest{TItem}" /> with properties copied from the source request.</returns>
    /// <remarks>
    ///     This conversion allows a general-purpose items provider request to be used in virtualization contexts. Note that the <see cref="CancellationToken" /> is set to the default value since it's
    ///     not available in the source request.
    /// </remarks>
    public static implicit operator TnTVirtualizeItemsProviderRequest<TItem>(TnTItemsProviderRequest request) {
        return new TnTVirtualizeItemsProviderRequest<TItem> {
            Count = request.Count,
            SortOnProperties = [.. request.SortOnProperties],
            StartIndex = request.StartIndex,
            CancellationToken = default
        };
    }
}

/// <summary>
///     A callback that provides data for a <see cref="TnTDataGrid{TGridItem}" />.
/// </summary>
/// <typeparam name="TItem">The type of data represented by each row in the grid.</typeparam>
/// <param name="request">Parameters describing the data being requested.</param>
/// <returns>A <see cref="ValueTask{TnTItemsProviderResult}" /> that gives the data to be displayed.</returns>
public delegate ValueTask<TnTItemsProviderResult<TItem>> TnTVirtualizeItemsProvider<TItem>(TnTVirtualizeItemsProviderRequest<TItem> request);