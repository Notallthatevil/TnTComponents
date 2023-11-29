using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace TnTComponents;
public class TnTElementScript : ComponentBase {
    [Parameter, EditorRequired]
    public string Src { get; set; } = default!;

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;


    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "tnt-element-script");
        builder.AddAttribute(10, "src", Src);
        builder.AddContent(20, ChildContent);
        builder.CloseElement();
    }
}
