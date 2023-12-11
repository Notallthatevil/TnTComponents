using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Text;
using TnTComponents.Enum;
using TnTComponents.Events;
using TnTComponents.Grid.Columns;

namespace TnTComponents.Grid.Infrastructure;
public partial class TnTDataGridCore<TGridItem> {
    [CascadingParameter]
    private TnTDataGridContext<TGridItem> _context { get; set; } = default!;

    [CascadingParameter]
    private ITnTDataGridSettings<TGridItem> _gridSettings { get; set; } = default!;

    private RenderFragment _renderHeaderContent;
    private RenderFragment _renderRowContent;

    private TnTColumnBase<TGridItem>? _sortedOn;
    private bool _ascending = true;

    public TnTDataGridCore() {
        _renderHeaderContent = RenderHeaderContent;
        _renderRowContent = RenderRowContent;
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
        Console.WriteLine($"Init {_context.Columns.Count}");

    }

    protected override void OnParametersSet() {
        base.OnParametersSet();
        Console.WriteLine($"Parameters {_context.Columns.Count}");
    }

    protected override void OnAfterRender(bool firstRender) {
        base.OnAfterRender(firstRender);

        if (firstRender) {
            StateHasChanged();
        }
        Console.WriteLine($"Render {_context.Columns.Count}");

    }

    private async Task RowClicked(MouseEventArgs args, object? item, int rowIndex) {
        await _gridSettings.RowClickedCallback.InvokeAsync(new DataGridRowClickEventArgs(args, item, rowIndex));
    }

    private void SortOn(TnTColumnBase<TGridItem> column) {
        if (_gridSettings.Items is not null) {
            if (_sortedOn == column && _ascending) {
                _gridSettings.Items = _gridSettings.Items.OrderByDescending(column.SortFunction!);
                _ascending = false;
            }
            else {
                _gridSettings.Items = _gridSettings.Items?.OrderBy(column.SortFunction!);
                _ascending = true;
            }
            StateHasChanged();
            _sortedOn = column;
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

}
