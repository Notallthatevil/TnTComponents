using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using TnTComponents.Core;

namespace TnTComponents;

public class TnTFabButton : TnTButton {
    [Parameter]
    public override int Elevation { get; set; } = 3;

    [Parameter]
    public override TnTBorderRadius? BorderRadius { get; set; } = new(4);

    protected override void BuildRenderTree(RenderTreeBuilder __builder) {
        __builder.OpenElement(0, "div");
        __builder.AddAttribute(10, "class", CssClassBuilder.Create().AddClass("tnt-fab").Build());
        __builder.AddContent(20, new RenderFragment(base.BuildRenderTree));
        __builder.CloseElement();
    }

}