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

    [Parameter]
    public TnTBadge? Badge { get; set; }

    public override TnTColor TextColor { get; set; } = TnTColor.OnSurface;

    protected override void OnParametersSet() {
        base.OnParametersSet();
        ChildContent = new RenderFragment(b => {
            b.AddContent(0, Icon.Render());
            if (Badge is not null) {
                b.OpenRegion(1);
                b.AddContent(0, Badge.Render);
                b.CloseRegion();
            }
        });

    }
}