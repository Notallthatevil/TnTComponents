using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TnTComponents.Core;

namespace TnTComponents;

public class TnTImageButton : TnTButton {

    [Parameter]
    public override ButtonAppearance Appearance { get; set; } = ButtonAppearance.Text;

    public override string? ElementClass => CssClassBuilder.Create(base.ElementClass!)
        .AddClass("tnt-image-button")
        .Build();

    [Parameter, EditorRequired]
    public TnTIcon Icon { get; set; } = default!;

    protected override void OnParametersSet() {
        base.OnParametersSet();
        ChildContent = Icon.Render();
    }
}