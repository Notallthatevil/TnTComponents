using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Text;

namespace TnTComponents.Forms;

public partial class TnTDropDown<TListItemType> {

    [Parameter]
    public IReadOnlySet<TListItemType>? DisabledItems { get; set; }

    [Parameter]
    public Func<TListItemType?, string>? ItemValueCallback { get; set; }

    [Parameter, EditorRequired]
    public ICollection<TListItemType> ListItems { get; set; } = default!;

    [Parameter]
    public RenderFragment<TListItemType>? ListItemTemplate { get; set; }

    protected override string GetCssClass() {
        return base.GetCssClass() + " dropdown";
    }

    protected override async Task OnFocusOutAsync(FocusEventArgs e) {
        await RemoveInputFocus();
        await base.OnFocusOutAsync(e);
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

    private bool IsItemDisabled(TListItemType? item) {
        return item is not null && (DisabledItems?.Contains(item) ?? false);
    }

    private async Task SelectItem(TListItemType? item) {
        if (item != null && !IsItemDisabled(item)) {
            CurrentValue = item;
            await OnFocusOutAsync(new FocusEventArgs());
        }
    }
}