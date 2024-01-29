using Microsoft.AspNetCore.Components;

namespace TnTComponents;

public partial class TnTSideNavHeadline {

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    public override string? Class => null;
}