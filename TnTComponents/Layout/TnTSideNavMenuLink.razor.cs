using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace TnTComponents;

public partial class TnTSideNavMenuLink {

    [CascadingParameter]
    private TnTSideNav _sideNav { get; set; } = default!;

    public override string? Class => null;

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

}