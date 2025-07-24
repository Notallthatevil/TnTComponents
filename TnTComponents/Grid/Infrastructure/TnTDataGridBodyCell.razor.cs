using Microsoft.AspNetCore.Components;

namespace TnTComponents.Grid.Infrastructure;
[CascadingTypeParameter(nameof(TGridItem))]
public partial class TnTDataGridBodyCell<TGridItem> {

    [Parameter, EditorRequired]
    public TGridItem? Item { get; set; }    

}
