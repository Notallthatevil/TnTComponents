using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace TnTComponents;

/// <summary>
///     A Blazor component that renders a custom script element with a specified source.
/// </summary>
public class TnTPageScript : ComponentBase {

    /// <summary>
    ///     Gets or sets the source URL of the script.
    /// </summary>
    [Parameter, EditorRequired]
    public string Src { get; set; } = default!;

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "tnt-page-script");
        builder.AddAttribute(1, "src", Src);
        builder.CloseElement();
    }
}