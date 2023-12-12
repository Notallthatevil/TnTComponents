using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using TnTComponents.Enum;
using TnTComponents.Events;
using TnTComponents.Grid.Columns;

namespace TnTComponents.Grid.Infrastructure;
public partial class TnTDataGridCore<TGridItem> {

    [CascadingParameter]
    private TnTDataGridContext<TGridItem> _context { get; set; } = default!;

    [CascadingParameter]
    private ITnTDataGridSettings<TGridItem> _gridSettings { get; set; } = default!;

    private RenderFragment _renderRowContent;

    private TnTColumnBase<TGridItem>? _sortedOn;

    [Inject]
    private NavigationManager _navMan { get; set; } = default!;

    private string _sortByParam => Uri.EscapeDataString(_context.DataGridName) + "sortonindex";
    private string _sortAscParam => Uri.EscapeDataString(_context.DataGridName) + "asc";

    public TnTDataGridCore() {
        _renderRowContent = RenderRowContent;
    }

    public void Dispose() {
        GC.SuppressFinalize(this);
        _navMan.LocationChanged -= Navigated;
    }

    public override string GetClass() {
        var strBuilder = new StringBuilder(_gridSettings.GetClass());

        if (_gridSettings.Appearance == DataGridAppearance.Default) {
            return strBuilder.ToString();
        }

        if ((_gridSettings.Appearance & DataGridAppearance.Stripped) != DataGridAppearance.Default) {
            strBuilder.Append(' ').Append("stripped");
        }

        if ((_gridSettings.Appearance & DataGridAppearance.Compat) != DataGridAppearance.Default) {
            strBuilder.Append(' ').Append("compact");
        }
        return strBuilder.ToString();
    }

    protected override void OnInitialized() {
        base.OnInitialized();
        _navMan.LocationChanged += Navigated!;
        Navigated(null!, null!);
    }

    private void Navigated(object sender, EventArgs e) {
        var queryParams = HttpUtility.ParseQueryString(new Uri(_navMan.Uri).Query);
        var sortBy = queryParams![_sortByParam];
        var ascending = queryParams[_sortAscParam];

        if (sortBy is not null && int.TryParse(sortBy, out var index)) {
            var column = _context.Columns[index];

            if (column is not null) {
                if (ascending is not null) {
                    _gridSettings.Items = _gridSettings.Items!.OrderBy(column.SortFunction!);
                }
                else {
                    _gridSettings.Items = _gridSettings.Items!.OrderByDescending(column.SortFunction!);
                }
                _sortedOn = column;
                StateHasChanged();
            }
        }
    }

    private string? GetContainerStyle() {
        var strBuilder = new StringBuilder();
        if (_gridSettings.MaxHeight.HasValue) {
            strBuilder.Append("max-height:");
            if (_gridSettings.MaxHeight.Value <= 1) {
                strBuilder.Append(_gridSettings.MaxHeight * 100).Append('%');
            }
            else {
                strBuilder.Append(_gridSettings.MaxHeight).Append("px");
            }
            strBuilder.Append(';');
        }
        if (_gridSettings.Height.HasValue) {
            strBuilder.Append("height:");
            if (_gridSettings.Height.Value <= 1) {
                strBuilder.Append(_gridSettings.Height * 100).Append('%');
            }
            else {
                strBuilder.Append(_gridSettings.Height).Append("px");
            }
            strBuilder.Append(';');
        }
        return strBuilder.Length > 0 ? strBuilder.ToString() : null;
    }

    private void DefaultSort(TnTColumnBase<TGridItem> column) {
        if (column.Sortable) {
            _gridSettings.Items = _gridSettings.Items!.OrderByDescending(column.SortFunction!);
            _sortedOn = column;
            StateHasChanged();
        }
    }

    private string BuildHref(bool sortable, int index) {
        if (index == 0) {
            Console.WriteLine(_sortByParam);
        }

        if (sortable) {
            var queryParams = HttpUtility.ParseQueryString(new Uri(_navMan.Uri).Query);

            var sortBy = queryParams![_sortByParam];
            var asc = queryParams![_sortAscParam];

            if (sortBy is not null && sortBy == index.ToString()) {
                if (asc is null) {
                    queryParams[_sortAscParam] = bool.TrueString;
                }
                else {
                    queryParams.Remove(_sortAscParam);
                }
            }
            else {
                queryParams[_sortByParam] = index.ToString();
                queryParams.Remove(_sortAscParam);
            }

            return new UriBuilder(_navMan.Uri) {
                Query = queryParams.ToString()
            }.ToString();
        }
        else {
            return string.Empty;
        }
    }
    private string GetAnchorClass(int index) {
        var queryParams = HttpUtility.ParseQueryString(new Uri(_navMan.Uri).Query);

        var sortBy = queryParams![_sortByParam];
        var asc = queryParams![_sortAscParam];

        var stringBuilder = new StringBuilder();
        if (sortBy == index.ToString()) {
            stringBuilder.Append("active");

            if (asc is not null) {
                stringBuilder.Append(' ').Append("ascending");
            }
        }
        return stringBuilder.ToString();
    }
}
