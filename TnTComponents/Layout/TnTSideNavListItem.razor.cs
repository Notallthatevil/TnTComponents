using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace TnTComponents.Layout;

public partial class TnTSideNavListItem {

    [Parameter]
    public override string BaseCssClass { get; set; } = "tnt-side-nav-list-item";

    [Parameter, EditorRequired]
    public string Label { get; set; }

    [Parameter]
    public string? Href { get; set; }

    [Parameter]
    public NavLinkMatch Match { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;
}