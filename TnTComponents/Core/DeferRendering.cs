using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace TnTComponents.Core;

/// <summary>
///     A component that defers the rendering of its child content.
/// </summary>
public sealed class DeferRendering : ComponentBase {

    /// <summary>
    ///     Gets or sets the content to be rendered.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder) => builder.AddContent(0, ChildContent);
}