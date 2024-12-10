using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Core;

namespace TnTComponents;

public enum IconSize {
    Small,
    Medium,
    Large,
    ExtraLarge
}

public enum IconAppearance {
    Default,
    Outlined,
    Round,
    Sharp
}

public abstract class TnTIcon : TnTComponentBase {

    [Parameter]
    public IconAppearance Appearance { get; set; } = IconAppearance.Default;

    [Parameter]
    public TnTColor Color { get; set; } = TnTColor.OnSurface;

    [Parameter]
    public IconSize Size { get; set; } = IconSize.Medium;

    [Parameter, EditorRequired]
    public string Icon { get; set; } = default!;

    protected TnTIcon() { }

    internal TnTIcon(string icon) {
        Icon = icon;
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "span");
        builder.AddMultipleAttributes(10, AdditionalAttributes);
        builder.AddAttribute(20, "class", ElementClass);
        builder.AddAttribute(30, "style", ElementStyle);
        builder.AddAttribute(40, "id", ElementId);
        builder.AddAttribute(50, "title", Icon);
        builder.AddElementReferenceCapture(60, e => Element = e);

        builder.AddContent(70, Icon);

        builder.CloseElement();
    }

    public RenderFragment Render() => new RenderFragment(BuildRenderTree);

    public static implicit operator string(TnTIcon icon) => icon.Icon;
}