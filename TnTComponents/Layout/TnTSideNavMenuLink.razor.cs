using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTSideNavMenuLink {

    [Parameter]
    public TnTColor ActiveBackgroundColor { get; set; } = TnTColor.SecondaryContainer;

    [Parameter]
    public TnTBorderRadius BorderRadius { get; set; } = new(10);

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    public override string? Class => CssBuilder.Create()
        .SetDisabled(Disabled)
        .AddBorderRadius(BorderRadius)
        .Build();

    [Parameter]
    public string? Href { get; set; }

    [Parameter]
    public TnTIcon? Icon { get; set; }

    [Parameter]
    public NavLinkMatch Match { get; set; }

    [Parameter]
    public bool Ripple { get; set; } = true;

    [Parameter]
    public string? Status { get; set; }

    [CascadingParameter]
    private TnTSideNav _sideNav { get; set; } = default!;
}