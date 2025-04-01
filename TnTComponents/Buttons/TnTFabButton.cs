using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using TnTComponents.Core;

namespace TnTComponents;

/// <summary>
///     Represents a floating action button component. This component sits in the bottom right hand corner of the screen and responds to multiple <see cref="TnTFabButton" /> buttons being added as
///     well as <see cref="TnTToast" /> components that may pop in.
/// </summary>
public class TnTFabButton : TnTButton {

    /// <inheritdoc />
    [Parameter]
    public override TnTBorderRadius? BorderRadius { get; set; } = new(4);

    /// <inheritdoc />
    [Parameter]
    public override int Elevation { get; set; } = 3;

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder __builder) {
        __builder.OpenElement(0, "div");
        __builder.AddAttribute(10, "class", CssClassBuilder.Create().AddClass("tnt-fab").Build());
        __builder.AddContent(20, new RenderFragment(base.BuildRenderTree));
        __builder.CloseElement();
    }
}