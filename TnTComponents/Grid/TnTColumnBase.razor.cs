using Microsoft.AspNetCore.Components;
using TnTComponents.Infrastructure;

namespace TnTComponents.Grid;
public abstract partial class TnTColumnBase<TGridItem> {
    [CascadingParameter]
    internal TnTGridContext Grid { get; set; } = default!;
    public string Class => GetCssClass();

    public RenderFragment HeaderContent => throw new NotImplementedException();

    protected override void OnInitialized() {
        if(Grid is null) {
            throw new InvalidOperationException($"A column must be a child of {nameof(TnTGrid<TGridItem>)}");
        }

        Grid.AddColumn(this);
    }

    public void Dispose() {
        Grid.RemoveColumn(this);
        GC.SuppressFinalize(this);
    }
}
