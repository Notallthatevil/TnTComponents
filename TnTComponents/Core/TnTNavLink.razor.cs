using Microsoft.AspNetCore.Components;
using TnTComponents.Core;
using TnTComponents.Enum;

namespace TnTComponents;

public partial class TnTNavLink {

    [Parameter]
    public TnTColor? ActiveBackgroundColor { get; set; }

    [Parameter]
    public TnTColor ActiveTextColor { get; set; } = TnTColor.OnSecondaryContainer;

    [Parameter]
    public NavLinkAppearance Appearance { get; set; }

    [Parameter]
    public bool? AutoFocus { get; set; }

    [Parameter]
    public TnTColor? BackgroundColor { get; set; } = TnTColor.Transparent;

    public string? CssClass => CssClassBuilder.Create()
                .AddClass(base.CssClass)
        .SetDisabled(Disabled)
        .AddRipple(Ripple)
        .AddActionableBackgroundColor(base.CssClass?.Contains("active") == true ? ActiveBackgroundColor : BackgroundColor)
        .AddForegroundColor(base.CssClass?.Contains("active") == true ? ActiveTextColor : TextColor)
        .Build();

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public ElementReference Element { get; set; }

    [Parameter]
    public TnTIcon? EndIcon { get; set; }

    [Parameter]
    public string? Id { get; set; }

    [Parameter]
    public bool Ripple { get; set; }

    [Parameter]
    public TnTIcon? StartIcon { get; set; }

    [Parameter]
    public string? CssStyle { get; set; }

    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnSurface;
}