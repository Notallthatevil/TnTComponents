using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTSideNavMenuLink {

    [CascadingParameter]
    private TnTSideNav _sideNav { get; set; } = default!;

    public override string? Class => CssBuilder.Create()
        .AddRipple(Ripple)
        .SetDisabled(Disabled)
        .AddBorderRadius(BorderRadius)
        .Build();

    [Parameter]
    public TnTIcon? Icon { get; set; }

    [Parameter]
    public string? Href { get; set; }

    [Parameter]
    public NavLinkMatch Match { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public string? Status { get; set; }

    [Parameter]
    public bool Ripple { get; set; } = true;

    [Parameter]
    public TnTBorderRadius BorderRadius { get; set; } = new(10);

    [Parameter]
    public TnTColor ActiveBackgroundColor { get; set; } = TnTColor.SecondaryContainer;

}