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
    public bool? AutoFocus { get; set; }

    [Parameter]
    public TnTColor? BackgroundColor { get; set; } = TnTColor.Transparent;

    [Parameter]
    public TnTBorderRadius? BorderRadius { get; set; } = new(10);

    [Parameter]
    public int Elevation { get; set; } = 0;

    [Parameter]
    public bool Outlined { get; set; }

    [Parameter, EditorRequired]
    public string Href { get; set; } = default!;

    public new string? CssClass => CssClassBuilder.Create()
        .AddClass(base.CssClass)
        .SetDisabled(Disabled)
        .AddRipple(Ripple)
        .AddActionableBackgroundColor(base.CssClass?.Contains("active") == true ? ActiveBackgroundColor : BackgroundColor)
        .AddForegroundColor(base.CssClass?.Contains("active") == true ? ActiveTextColor : TextColor)
        .AddOutlined(Outlined)
        .AddFilled(BackgroundColor != TnTColor.Transparent)
        .AddElevation(Elevation)
        .AddBorderRadius(BorderRadius)
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