using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace TnTComponents;

public class TnTPageScript : ComponentBase {

    [Parameter, EditorRequired]
    public string Src { get; set; } = default!;

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "tnt-page-script");
        builder.AddAttribute(1, "src", Src);
        builder.CloseElement();
    }
}