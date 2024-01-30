using Microsoft.AspNetCore.Components;
using TnTComponents.Common;
using TnTComponents.Core;
using TnTComponents.Enum;

namespace TnTComponents;

public partial class TnTNavLink {

    [Parameter]
    public NavLinkAppearance Appearance { get; set; }

    public string? Class => CssBuilder.Create()
        .AddClass(CssClass)
        .SetDisabled(Disabled)
        .AddRipple(Ripple)
        .AddActionableBackgroundColor(CssClass?.Contains("active") == true ? ActiveBackgroundColor : BackgroundColor)
        .AddForegroundColor(CssClass?.Contains("active") == true ? ActiveTextColor : TextColor)
        .Build();

    [Parameter]
    public TnTIcon? EndIcon { get; set; }

    [Parameter]
    public TnTIcon? StartIcon { get; set; }

    [Parameter]
    public ElementReference Element { get; set; }

    [Parameter]
    public string? Style { get; set; }

    [Parameter]
    public string? Id { get; set; }
    [Parameter]
    public bool? AutoFocus { get; set; }
    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public bool Ripple { get; set; }

    [Parameter]
    public TnTColor? BackgroundColor { get; set; } = TnTColor.Transparent;

    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnSurface;

    [Parameter]
    public TnTColor ActiveTextColor { get; set; } = TnTColor.OnSecondaryContainer;

    [Parameter]
    public TnTColor? ActiveBackgroundColor { get; set; }
}