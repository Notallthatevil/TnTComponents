using Microsoft.AspNetCore.Components;
using System.Text;
using TnTComponents.Enum;

namespace TnTComponents;

public partial class TnTStack {

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public Orientation Direction { get; set; } = Orientation.Vertical;

    [Parameter]
    public bool Reverse { get; set; } = false;

    [Parameter]
    public int Gap { get; set; } = 10;

    private string GetStyle() {
        var strBuilder = new StringBuilder($"display: flex;gap: {Gap}px;");

        strBuilder.Append($"flex-direction: {(Direction == Orientation.Vertical ? "column" : "row")}{(Reverse ? "-reverse" : string.Empty)};");

        return strBuilder.ToString();
    }
}