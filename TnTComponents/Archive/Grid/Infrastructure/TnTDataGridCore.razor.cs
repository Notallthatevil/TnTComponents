//using Microsoft.AspNetCore.Components;
//using Microsoft.JSInterop;
//using System.Text;
//using System.Web;
//using TnTComponents.Archive.Grid.Columns;
//using TnTComponents.Enum;

//namespace TnTComponents.Archive.Grid.Infrastructure;

//public partial class TnTDataGridCore<TGridItem> {
//    [CascadingParameter]
//    private TnTDataGridContext<TGridItem> _context { get; set; } = default!;

// [CascadingParameter] private ITnTDataGridSettings<TGridItem> _gridSettings { get; set; } = default!;

// [Inject] private NavigationManager _navMan { get; set; } = default!;

// [Inject] private IJSRuntime _jsRuntime { get; set; } = default!;

// private IJSObjectReference? _module;

// private string _sortAscParam => Uri.EscapeDataString(_context.DataGridName) + "asc"; private
// string _sortByParam => Uri.EscapeDataString(_context.DataGridName) + "sortonindex";

// private TnTColumnBase<TGridItem>? _sortedOn;

// public override string GetClass() { var strBuilder = new StringBuilder(_gridSettings.GetClass());

// if (_gridSettings.Appearance == DataGridAppearance.Default) { return strBuilder.ToString(); }

// if ((_gridSettings.Appearance & DataGridAppearance.Stripped) != DataGridAppearance.Default) {
// strBuilder.Append(' ').Append("stripped"); }

// if ((_gridSettings.Appearance & DataGridAppearance.Compact) != DataGridAppearance.Default) {
// strBuilder.Append(' ').Append("compact"); } return strBuilder.ToString(); }

// protected override void OnInitialized() { base.OnInitialized(); _navMan.LocationChanged +=
// Navigated!; }

// protected override void OnParametersSet() { base.OnParametersSet(); Navigated(this,
// EventArgs.Empty); }

// private string BuildHref(bool sortable, int index) { if (sortable) { var queryParams =
// HttpUtility.ParseQueryString(new Uri(_navMan.Uri).Query);

// var sortBy = queryParams![_sortByParam]; var asc = queryParams![_sortAscParam];

// if (sortBy is not null && sortBy == index.ToString()) { if (asc is null) {
// queryParams[_sortAscParam] = bool.TrueString; } else { queryParams.Remove(_sortAscParam); } }
// else { queryParams[_sortByParam] = index.ToString(); queryParams.Remove(_sortAscParam); }

// return new UriBuilder(_navMan.Uri) { Query = queryParams.ToString() }.ToString(); } else { return
// string.Empty; } }

// private void DefaultSort(TnTColumnBase<TGridItem> column) { if (column.Sortable) {
// _gridSettings.Items = _gridSettings.Items!.OrderByDescending(column.SortFunction!); _sortedOn =
// column; StateHasChanged(); } }

// private string GetAnchorClass(int index) { var queryParams = HttpUtility.ParseQueryString(new Uri(_navMan.Uri).Query);

// var sortBy = queryParams![_sortByParam]; var asc = queryParams![_sortAscParam];

// var stringBuilder = new StringBuilder(); if (sortBy == index.ToString()) { stringBuilder.Append("active");

// if (asc is not null) { stringBuilder.Append(' ').Append("ascending"); } } return
// stringBuilder.ToString(); }

// private string? GetContainerStyle() { var strBuilder = new StringBuilder(); if
// (_gridSettings.MaxHeight.HasValue) { strBuilder.Append("max-height:"); if
// (_gridSettings.MaxHeight.Value <= 1) { strBuilder.Append(_gridSettings.MaxHeight *
// 100).Append('%'); } else { strBuilder.Append(_gridSettings.MaxHeight).Append("px"); }
// strBuilder.Append(';'); } if (_gridSettings.Height.HasValue) { strBuilder.Append("height:"); if
// (_gridSettings.Height.Value <= 1) { strBuilder.Append(_gridSettings.Height * 100).Append('%'); }
// else { strBuilder.Append(_gridSettings.Height).Append("px"); } strBuilder.Append(';'); } return
// strBuilder.Length > 0 ? strBuilder.ToString() : null; }

// private void Navigated(object sender, EventArgs e) { var queryParams =
// HttpUtility.ParseQueryString(new Uri(_navMan.Uri).Query); var sortBy =
// queryParams![_sortByParam]; var ascending = queryParams[_sortAscParam];

// if (sortBy is not null && int.TryParse(sortBy, out var index)) { var column = _context.Columns[index];

//            if (column is not null) {
//                if (ascending is not null) {
//                    _gridSettings.Items = _gridSettings.Items!.OrderBy(column.SortFunction!);
//                }
//                else {
//                    _gridSettings.Items = _gridSettings.Items!.OrderByDescending(column.SortFunction!);
//                }
//                _sortedOn = column;
//                StateHasChanged();
//            }
//        }
//    }
//}