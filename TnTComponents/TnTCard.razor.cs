using Microsoft.AspNetCore.Components;
using TnTComponents.Enum;

namespace TnTComponents;

public partial class TnTCard {

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public override string? Class { get; set; } = "tnt-card";

    [Parameter]
    public CardType Type { get; set; }

    public override string GetClass() {
        return base.GetClass() + " " + Type.ToString().ToLower();
    }
}