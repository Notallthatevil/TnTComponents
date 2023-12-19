using Microsoft.AspNetCore.Components;

namespace TnTComponents.Layout;
public partial class TnTSideNavHeadline {
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public override string? Class { get; set; } = "tnt-side-nav-headline";
}
