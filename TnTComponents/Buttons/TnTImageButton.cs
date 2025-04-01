using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TnTComponents.Core;

namespace TnTComponents;

/// <summary>
///     Represents an image button component with customizable icon and optional badge.
/// </summary>
public class TnTImageButton : TnTButton {

    /// <inheritdoc />
    [Parameter]
    public override ButtonAppearance Appearance { get; set; } = ButtonAppearance.Text;

    /// <summary>
    ///     The optional badge to be displayed with the icon.
    /// </summary>
    [Parameter]
    public TnTBadge? Badge { get; set; }

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create(base.ElementClass!)
        .AddClass("tnt-image-button")
        .Build();

    /// <summary>
    ///     The icon to be displayed in the button.
    /// </summary>
    [Parameter, EditorRequired]
    public TnTIcon Icon { get; set; } = default!;

    /// <inheritdoc />
    public override TnTColor TextColor { get; set; } = TnTColor.OnSurface;

    /// <summary>
    ///     Sets the parameters for the component and updates the child content with the icon and badge.
    /// </summary>
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