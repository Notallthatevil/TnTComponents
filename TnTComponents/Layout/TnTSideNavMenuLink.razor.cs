using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace TnTComponents.Layout;
public partial class TnTSideNavMenuLink {
    [CascadingParameter]
    private TnTSideNav _sideNav { get; set; } = default!;


    [Parameter]
    public override string? Class { get; set; } = "tnt-side-nav-menu-link";

    [Parameter]
    public string? Icon { get; set; }

    [Parameter]
    public string? Href { get; set; }

    [Parameter]
    public NavLinkMatch Match { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public string? Status { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

}
