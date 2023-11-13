using Microsoft.AspNetCore.Components;
using TnTComponents.Enum;

namespace TnTComponents;
public partial class TnTCard {
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;


    [Parameter]
    public CardType CardType { get; set; }

    [Parameter]
    public override string BaseCssClass { get; set; } = "tnt-card";


    protected override string GetCssClass() {
        return $"{base.GetCssClass()} {CardType switch {
            CardType.Filled => "filled",
            CardType.Elevated => "elevated",
            CardType.Outlined => "outlined",
            _ => string.Empty,
        }}";
    }
}