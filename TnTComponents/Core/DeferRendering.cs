using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace TnTComponents.Core;

public sealed class DeferRendering : ComponentBase {
    [Parameter] public RenderFragment? ChildContent { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.AddContent(0, ChildContent);
    }
}