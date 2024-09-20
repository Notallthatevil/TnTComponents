using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

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

public abstract class TnTIcon : ComponentBase {
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    public abstract string? ElementClass { get; }
    public abstract string? ElementStyle { get; }

    [Parameter]
    public IconAppearance Appearance { get; set; } = IconAppearance.Default;

    [Parameter]
    public TnTColor Color { get; set; } = TnTColor.OnSurface;

    [Parameter]
    public IconSize Size { get; set; } = IconSize.Medium;

    [Parameter, EditorRequired]
    public string? Icon { get; set; }

    public string? AdditionalClass { get; set; }

    public ElementReference Element { get; private set; }

    protected TnTIcon() { }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "span");
        builder.AddMultipleAttributes(10, AdditionalAttributes);
        builder.AddAttribute(20, "class", ElementClass);
        builder.AddAttribute(30, "style", ElementStyle);
        builder.AddElementReferenceCapture(40, e => Element = e);

        builder.AddContent(60, Icon);

        builder.CloseElement();
    }

    public RenderFragment Render() => new RenderFragment(BuildRenderTree);
}

