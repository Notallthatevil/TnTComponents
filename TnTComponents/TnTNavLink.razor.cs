using Microsoft.AspNetCore.Components;
using TnTComponents.Common;
using TnTComponents.Enum;

namespace TnTComponents;

public partial class TnTNavLink {

    [Parameter]
    public NavLinkAppearance Appearance { get; set; }

    [Parameter]
    public string? Class { get; set; } = "tnt-nav-link";

    [Parameter]
    public string? EndIcon { get; set; }

    [Parameter]
    public IconType IconType { get; set; }

    [Parameter]
    public string? StartIcon { get; set; }

    [Parameter]
    public ElementReference Element { get; set; }

    [Parameter]
    public string? Id { get; set; }

    [Parameter]
    public string? Style { get; set; }

    [Parameter]
    public string? Theme { get; set; }

    public string GetClass() => $"{this.GetClassDefault()} {Appearance.ToString().ToLower()} {CssClass}";
}