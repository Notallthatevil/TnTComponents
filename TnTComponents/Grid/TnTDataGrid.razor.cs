using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using TnTComponents.Core;
using TnTComponents.Ext;
using TnTComponents.Grid;
using TnTComponents.Grid.Columns;
using TnTComponents.Grid.Infrastructure;
using TnTComponents.Virtualization;

namespace TnTComponents;

/// <summary>
/// A component that displays a grid.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
[CascadingTypeParameter(nameof(TGridItem))]
public partial class TnTDataGrid<TGridItem> {

    /// <summary>
    /// Gets or sets the child components of this instance. For example, you may define columns by
    /// adding components derived from the <see cref="TnTColumnBase{TGridItem}" /> base class.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddBackgroundColor(BackgroundColor)
        .AddFilled(BackgroundColor is not null)
        .AddClass("tnt-datagrid")
        .AddClass("tnt-stripped", DataGridAppearance.HasFlag(DataGridAppearance.Stripped))
        .AddClass("tnt-compact", DataGridAppearance.HasFlag(DataGridAppearance.Compact))
        .AddClass("tnt-resizable", Resizable)
        .AddClass("tnt-loading", Loading)
        .Build();

    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    [Parameter]
    public TnTColor? BackgroundColor { get; set; } = TnTColor.Background;

    [Parameter]
    public DataGridAppearance DataGridAppearance { get; set; }

    /// <summary>
    /// If specified, grids render this fragment when there is no content.
    /// </summary>
    [Parameter]
    public RenderFragment? EmptyContent { get; set; }

    /// <summary>
    /// Gets or sets the value that gets applied to the css gridTemplateColumns attribute of child rows.
    /// </summary>
    [Parameter]
    public string? GridTemplateColumns { get; set; } = null;

    /// <summary>
    /// Optionally defines a value for @key on each rendered row. Typically this should be used to
    /// specify a unique identifier, such as a primary key value, for each data item. /// This
    /// allows the grid to preserve the association between row elements and data items based on
    /// their unique identifiers, even when the <typeparamref name="TGridItem" /> instances are
    /// replaced by new copies (for example, after a new query against the underlying data store).
    /// /// If not set, the @key will be the <typeparamref name="TGridItem" /> instance itself.
    /// </summary>
    [Parameter]
    public Func<TGridItem, object> ItemKey { get; set; } = x => x!;

    /// <summary>
    /// Gets or sets a queryable source of data for the grid. /// This could be in-memory data
    /// converted to queryable using the <see
    /// cref="System.Linq.Queryable.AsQueryable(System.Collections.IEnumerable)" /> extension
    /// method, or an EntityFramework DataSet or an <see cref="IQueryable" /> derived from it. ///
    /// You should supply either <see cref="Items" /> or <see cref="ItemsProvider" />, but not both.
    /// </summary>
    [Parameter]
    public IQueryable<TGridItem>? Items { get; set; }

    /// <summary>
    /// This is applicable only when using <see cref="Virtualize" />. It defines an expected height
    /// in pixels for each row, allowing the virtualization mechanism to fetch the correct number of
    /// items to match the display size and to ensure accurate scrolling.
    /// </summary>
    [Parameter]
    public float ItemSize { get; set; } = 32;

    /// <summary>
    /// Gets or sets a callback that supplies data for the rid. /// You should supply either <see
    /// cref="Items" /> or <see cref="ItemsProvider" />, but not both.
    /// </summary>
    [Parameter]
    public TnTGridItemsProvider<TGridItem>? ItemsProvider { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the grid is in a loading data state.
    /// </summary>
    [Parameter]
    public bool Loading { get; set; }

    /// <summary>
    /// Gets or sets the content to render when <see cref="Loading" /> is true. A default fragment
    /// is used if loading content is not specified.
    /// </summary>
    [Parameter]
    public RenderFragment? LoadingContent { get; set; }

    [Parameter]
    public EventCallback<TGridItem?> OnRowClicked { get; set; }

    /// <summary>
    /// Optionally links this <see cref="TnTDataGrid{TGridItem}" /> instance with a <see
    /// cref="TnTPaginationState" /> model, causing the grid to fetch and render only the current
    /// page of data. /// This is normally used in conjunction with a <see cref="FluentPaginator" />
    /// component or some other UI logic that displays and updates the supplied <see
    /// cref="TnTPaginationState" /> instance.
    /// </summary>
    [Parameter]
    public TnTPaginationState? Pagination { get; set; }

    /// <summary>
    /// If true, renders draggable handles around the column headers, allowing the user to resize
    /// the columns manually. Size changes are not persisted.
    /// </summary>
    [Parameter]
    public bool Resizable { get; set; }

    /// <summary>
    /// Gets or sets a value that determines how many additional items will be rendered
    /// before and after the visible region. This help to reduce the frequency of rendering
    /// during scrolling. However, higher values mean that more elements will be present
    /// in the page.
    /// </summary>
    [Parameter]
    public int OverscanCount { get; set; } = 3;

    /// <summary>
    /// Optionally defines a class to be applied to a rendered row.
    /// </summary>
    [Parameter]
    public Func<TGridItem, string>? RowClass { get; set; }

    /// <summary>
    /// Optionally defines a style to be applied to a rendered row. Do not use to dynamically update
    /// a row style after rendering as this will interfere with the script that use this attribute.
    /// Use <see cref="RowClass" /> instead.
    /// </summary>
    [Parameter]
    public Func<TGridItem, string>? RowStyle { get; set; }

    public bool SortByAscending => _sortByAscending;

    /// <summary>
    /// If true, the grid will be rendered with virtualization. This is normally used in conjunction
    /// with scrolling and causes the grid to fetch and render only the data around the current
    /// scroll viewport. This can greatly improve the performance when scrolling through large data
    /// sets. /// If you use <see cref="Virtualize" />, you should supply a value for <see
    /// cref="ItemSize" /> and must ensure that every row renders with the same constant height. ///
    /// Generally it's preferable not to use <see cref="Virtualize" /> if the amount of data being
    /// rendered is small or if you are using pagination.
    /// </summary>
    [Parameter]
    public bool Virtualize { get; set; }

    [Inject]
    private IServiceProvider Services { get; set; } = default!;

    public override string? JsModulePath => "./_content/TnTComponents/Grid/TnTDataGrid.razor.js";

    private readonly List<TnTColumnBase<TGridItem>> _columns;

    // If the PaginationState mutates, it raises this event. We use it to trigger a re-render.
    private readonly EventCallbackSubscriber<TnTPaginationState> _currentPageItemsChanged;

    // We cascade the InternalGridContext to descendants, which in turn call it to add themselves to
    // _columns This happens on every render so that the column list can be updated dynamically
    private readonly TnTInternalGridContext<TGridItem> _internalGridContext;

    private readonly RenderFragment _renderEmptyContent;
    private readonly RenderFragment _renderLoadingContent;
    private readonly RenderFragment _renderNonVirtualizedRows;
    private int _ariaBodyRowCount = -1;

    private int _numberOfRowsToLoad = 5;

    // IQueryable only exposes synchronous query APIs. IAsyncQueryExecutor is an adapter that lets
    // us invoke any async query APIs that might be available. We have built-in support for using EF
    // Core's async query APIs.
    private IAsyncQueryExecutor? _asyncQueryExecutor;

    private bool _collectingColumns;
    private IReadOnlyCollection<TGridItem> _currentNonVirtualizedViewItems = Array.Empty<TGridItem>();
    private int _delay = 100;
    private bool _interactive;

    private object? _lastAssignedItemsOrProvider;

    // We try to minimize the number of times we query the items provider, since queries may be
    // expensive We only re-query when the developer calls RefreshDataAsync, or if we know
    // something's changed, such as sort order, the pagination state, or the data source itself.
    // These fields help us detect when things have changed, and to discard earlier load attempts
    // that were superseded.
    private int? _lastRefreshedPaginationStateHash;

    private bool _manualGrid;
    private CancellationTokenSource? _pendingDataLoadCancellationTokenSource;
    private bool _sortByAscending;

    // Columns might re-render themselves arbitrarily. We only want to capture them at a defined time.
    private TnTColumnBase<TGridItem>? _sortByColumn;

    private TnTVirtualize<(int, TGridItem)>? _virtualizeComponent;

    private bool _loading = true;

    /// <summary>
    /// Constructs an instance of <see cref="TnTDataGrid{TGridItem}" />.
    /// </summary>
    public TnTDataGrid() {
        _columns = [];
        _internalGridContext = new(this);
        _currentPageItemsChanged = new(EventCallback.Factory.Create<TnTPaginationState>(this, RefreshDataCoreAsync));
        _renderNonVirtualizedRows = RenderNonVirtualizedRows;
        _renderEmptyContent = RenderEmptyContent;
        _renderLoadingContent = RenderLoadingContent;

        // As a special case, we don't issue the first data load request until we've collected the
        // initial set of columns This is so we can apply default sort order (or any future
        // per-column options) before loading data We use EventCallbackSubscriber to safely hook
        // this async operation into the synchronous rendering flow
        EventCallbackSubscriber<object?>? columnsFirstCollectedSubscriber = new(
            EventCallback.Factory.Create<object?>(this, RefreshDataCoreAsync));
        columnsFirstCollectedSubscriber.SubscribeOrMove(_internalGridContext.ColumnsFirstCollected);
    }

    /// <summary>
    /// Instructs the grid to re-fetch and render the current data from the supplied data source
    /// (either <see cref="Items" /> or <see cref="ItemsProvider" />).
    /// </summary>
    /// <returns>A <see cref="Task" /> that represents the completion of the operation.</returns>
    public async Task RefreshDataAsync() {
        await RefreshDataCoreAsync();
    }

    /// <summary>
    /// Sets the grid's current sort column to the specified <paramref name="column" />.
    /// </summary>
    /// <param name="column">The column that defines the new sort order.</param>
    /// <param name="direction">
    /// The direction of sorting. If the value is <see cref="SortDirection.Auto" />, then it will
    /// toggle the direction on each call.
    /// </param>
    /// <returns>A <see cref="Task" /> representing the completion of the operation.</returns>
    public Task SortByColumnAsync(TnTColumnBase<TGridItem> column, SortDirection direction = SortDirection.Auto) {
        _sortByAscending = direction switch {
            SortDirection.Ascending => true,
            SortDirection.Descending => false,
            SortDirection.Auto => _sortByColumn != column || !_sortByAscending,
            _ => throw new NotSupportedException($"Unknown sort direction {direction}"),
        };

        _sortByColumn = column;

        StateHasChanged(); // We want to see the updated sort order in the header, even before the data query is completed
        return RefreshDataAsync();
    }

    // Invoked by descendant columns at a special time during rendering
    internal void AddColumn(TnTColumnBase<TGridItem> column, SortDirection? initialSortDirection, bool isDefaultSortColumn) {
        if (_collectingColumns) {
            _columns.Add(column);

            if (isDefaultSortColumn && _sortByColumn is null && initialSortDirection.HasValue) {
                _sortByColumn = column;
                _sortByAscending = initialSortDirection.Value != SortDirection.Descending;
            }
        }
    }

    /// <inheritdoc />
    protected override Task OnParametersSetAsync() {
        // The associated pagination state may have been added/removed/replaced
        _currentPageItemsChanged.SubscribeOrMove(Pagination?.CurrentPageItemsChanged);

        if (Items is not null && ItemsProvider is not null) {
            throw new InvalidOperationException($"{nameof(TnTDataGrid<TGridItem>)} requires one of {nameof(Items)} or {nameof(ItemsProvider)}, but both were specified.");
        }

        // Perform a re-query only if the data source or something else has changed
        var _newItemsOrItemsProvider = Items ?? (object?)ItemsProvider;
        var dataSourceHasChanged = _newItemsOrItemsProvider != _lastAssignedItemsOrProvider;
        if (dataSourceHasChanged) {
            _lastAssignedItemsOrProvider = _newItemsOrItemsProvider;
            _asyncQueryExecutor = AsyncQueryExecutorSupplier.GetAsyncQueryExecutor(Services, Items);
        }

        var mustRefreshData = dataSourceHasChanged || (Pagination?.GetHashCode() != _lastRefreshedPaginationStateHash);

        // We don't want to trigger the first data load until we've collected the initial set of
        // columns, because they might perform some action like setting the default sort order, so
        // it would be wasteful to have to re-query immediately
        return (_columns.Count > 0 && mustRefreshData) ? RefreshDataCoreAsync() : Task.CompletedTask;
    }

    protected override void OnAfterRender(bool firstRender) {
        base.OnAfterRender(firstRender);
        if (firstRender) {
            _interactive = true;
        }
    }

    private void FinishCollectingColumns() {
        _collectingColumns = false;
        _manualGrid = _columns.Count == 0;
    }

    // Gets called both by RefreshDataCoreAsync and directly by the Virtualize child component
    // during scrolling
    private async ValueTask<TnTItemsProviderResult<(int, TGridItem)>> ProvideVirtualizedItemsAsync(TnTVirtualizeItemsProviderRequest<(int, TGridItem)> request) {
        _lastRefreshedPaginationStateHash = Pagination?.GetHashCode();
        // Debounce the requests. This eliminates a lot of redundant queries at the cost of slight
        // lag after interactions.
        // TODO: Consider making this configurable, or smarter (e.g., doesn't delay on first call in a batch, then the amount
        // of delay increases if you rapidly issue repeated requests, such as when scrolling a long way)
        await Task.Delay(_delay);
        if (_delay < 2000) {
            Interlocked.Add(ref _delay, 100);
        }
        var result = default(TnTItemsProviderResult<(int, TGridItem)>);
        if (!request.CancellationToken.IsCancellationRequested) {
            // Combine the query parameters from Virtualize with the ones from PaginationState
            var startIndex = request.StartIndex;
            var count = request.Count;
            if (Pagination is not null) {
                startIndex += Pagination.CurrentPageIndex * Pagination.ItemsPerPage;
                count = Math.Min(request.Count.GetValueOrDefault(1), Pagination.ItemsPerPage - request.StartIndex);
            }

            TnTGridItemsProviderRequest<TGridItem> providerRequest = new(startIndex, count, _sortByColumn, _sortByAscending, request.CancellationToken);
            var providerResult = await ResolveItemsRequestAsync(providerRequest);

            if (!request.CancellationToken.IsCancellationRequested) {
                // ARIA's rowcount is part of the UI, so it should reflect what the human user
                // regards as the number of rows in the table, not the number of physical <tr>
                // elements. For virtualization this means what's in the entire scrollable range,
                // not just the current viewport. In the case where you're also paginating then it
                // means what's conceptually on the current page.
                // TODO: This currently assumes we always want to expand the last page to have ItemsPerPage rows, but the experience might
                // be better if we let the last page only be as big as its number of actual rows.
                _ariaBodyRowCount = Pagination is null ? providerResult.TotalItemCount : Pagination.ItemsPerPage;

                Pagination?.SetTotalItemCountAsync(providerResult.TotalItemCount);
                if (_ariaBodyRowCount > 0) {
                    Loading = false;
                }

                // We're supplying the row _index along with each row's data because we need it for
                // aria-rowindex, and we have to account for the virtualized start _index. It might
                // be more performant just to have some _latestQueryRowStartIndex field, but we'd
                // have to make sure it doesn't get out of sync with the rows being rendered.
                result = new TnTItemsProviderResult<(int, TGridItem)>(
                     items: providerResult.Items.Select((x, i) => ValueTuple.Create(i + request.StartIndex + 2, x)).ToList(),
                     totalCount: _ariaBodyRowCount);
            }

            Interlocked.Exchange(ref _delay, 0); // Reset the debounce delay
        }

        return result;
    }

    // Same as RefreshDataAsync, except without forcing a re-render. We use this from
    // OnParametersSetAsync because in that case there's going to be a re-render anyway.
    private async Task RefreshDataCoreAsync() {
        // Move into a "loading" state, cancelling any earlier-but-still-pending load
        _pendingDataLoadCancellationTokenSource?.Cancel();
        var thisLoadCts = _pendingDataLoadCancellationTokenSource = new CancellationTokenSource();

        if (_virtualizeComponent is not null) {
            // If we're using Virtualize, we have to go through its RefreshDataAsync API otherwise:
            // (1) It won't know to update its own internal state if the provider output has changed
            // (2) We won't know what slice of data to query for
            await _virtualizeComponent.RefreshDataAsync();
            _pendingDataLoadCancellationTokenSource = null;
        }
        else {
            // If we're not using Virtualize, we build and execute a request against the items
            // provider directly
            _lastRefreshedPaginationStateHash = Pagination?.GetHashCode();

            var startIndex = Pagination is null ? 0 : (Pagination.CurrentPageIndex * Pagination.ItemsPerPage);
            var request = new TnTGridItemsProviderRequest<TGridItem>(startIndex, Pagination?.ItemsPerPage, _sortByColumn, _sortByAscending, thisLoadCts.Token);

            var result = await ResolveItemsRequestAsync(request);
            if (!thisLoadCts.IsCancellationRequested) {
                _currentNonVirtualizedViewItems = result.Items;
                _ariaBodyRowCount = _currentNonVirtualizedViewItems.Count;
                Pagination?.SetTotalItemCountAsync(result.TotalItemCount);
                _pendingDataLoadCancellationTokenSource = null;
            }
            _internalGridContext.ResetRowIndexes(startIndex);
        }

        StateHasChanged();
    }

    // Normalizes all the different ways of configuring a data source so they have common
    // GridItemsProvider-shaped API
    private async ValueTask<TnTItemsProviderResult<TGridItem>> ResolveItemsRequestAsync(TnTGridItemsProviderRequest<TGridItem> request) {
        _loading = true;
        TnTItemsProviderResult<TGridItem> providerResult = new();
        if (ItemsProvider is not null) {
            if (Virtualize && request.Count is null) {
                if (IsolatedJsModule is not null) {
                    var bodyHeight = await IsolatedJsModule.InvokeAsync<int>("getBodyHeight", Element);

                    if (bodyHeight >= 0) {
                        _numberOfRowsToLoad = Math.Max(bodyHeight / (int)ItemSize, 5);
                    }
                }
                Console.WriteLine($"Number of rows to load {_numberOfRowsToLoad + OverscanCount}");
                request = request with { Count = _numberOfRowsToLoad + OverscanCount };
            }
            var gipr = await ItemsProvider(request);
            if (gipr.Items is not null) {
                Loading = false;
            }
            providerResult = gipr;
        }
        else if (Items is not null) {
            var totalItemCount = _asyncQueryExecutor is null ? Items.Count() : await _asyncQueryExecutor.CountAsync(Items);
            _ariaBodyRowCount = totalItemCount;
            var result = request.ApplySorting(Items).Skip(request.StartIndex);
            if (request.Count.HasValue) {
                result = result.Take(request.Count.Value);
            }
            var resultArray = _asyncQueryExecutor is null ? [.. result] : await _asyncQueryExecutor.ToArrayAsync(result);
            providerResult = new TnTItemsProviderResult<TGridItem> { Items = resultArray, TotalItemCount = totalItemCount };
        }
        _loading = false;
        return providerResult;
    }

    private void StartCollectingColumns() {
        _columns.Clear();
        _collectingColumns = true;
    }
}

/// <summary>
/// A callback that provides data for a <see cref="TnTDataGrid{TGridItem}" />.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
/// <param name="request">Parameters describing the data being requested.</param>
/// <returns>
/// A <see cref="T:ValueTask{TnTGridItemsProviderResult{TResult}}" /> that gives the data to be displayed.
/// </returns>
public delegate ValueTask<TnTItemsProviderResult<TGridItem>> TnTGridItemsProvider<TGridItem>(TnTGridItemsProviderRequest<TGridItem> request);