using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using TnTComponents.Common.Ext;

namespace TnTComponents.Forms;
public partial class TnTDropDown<TListItemType> {

    [Parameter, EditorRequired]
    public ICollection<TListItemType> ListItems { get; set; } = default!;

    [Parameter]
    public IReadOnlySet<TListItemType>? DisabledItems { get; set; }

    [Parameter]
    public RenderFragment<TListItemType>? ListItemTemplate { get; set; }

    [Parameter]
    public Func<TListItemType?, string>? ItemValueCallback { get; set; }

    protected override async Task OnFocusOutAsync(FocusEventArgs e) {
        await RemoveInputFocus();
        await base.OnFocusOutAsync(e);
    }

    protected override string GetCssClass() {
        return base.GetCssClass() + " dropdown";
    }

    private bool IsItemDisabled(TListItemType? item) {
        return item is not null && (DisabledItems?.Contains(item) ?? false);
    }

    private string GetItemClass(TListItemType? item) {
        var strBuilder = new StringBuilder();

        if (IsItemDisabled(item)) {
            strBuilder.Append("disabled");
        }

        if (item?.Equals(CurrentValue) ?? false) {
            strBuilder.Append(' ').Append("selected");
        }

        return strBuilder.ToString();
    }

    private string GetItemValue(TListItemType? item) {
        if (ItemValueCallback is not null) {
            return ItemValueCallback(item) ?? string.Empty;
        }
        else {
            return item?.ToString() ?? string.Empty;
        }
    }

    private async Task SelectItem(TListItemType? item) {
        if (item != null && !IsItemDisabled(item)) {
            CurrentValue = item;
            await OnFocusOutAsync(new FocusEventArgs());
        }
    }

}
