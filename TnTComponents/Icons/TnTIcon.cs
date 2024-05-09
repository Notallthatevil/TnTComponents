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
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    public abstract string? CssClass { get; }
    public abstract string? CssStyle { get; }

    [Parameter]
    public IconAppearance Appearance { get; set; } = IconAppearance.Default;

    [Parameter]
    public TnTColor Color { get; set; } = TnTColor.OnSurface;

    [Parameter]
    public IconSize Size { get; set; } = IconSize.Medium;

    public string? Icon { get; }

    public string? AdditionalClass { get; set; }

    public ElementReference Element { get; private set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    protected TnTIcon() { }
    protected TnTIcon(string icon) {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(icon, nameof(icon));
        Icon = icon;
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "span");
        builder.AddMultipleAttributes(10, AdditionalAttributes);
        builder.AddAttribute(20, "class", CssClass);
        builder.AddAttribute(30, "style", CssStyle);
        builder.AddElementReferenceCapture(40, e => Element = e);

        if (ChildContent is not null) {
            builder.AddContent(50, ChildContent);
        }
        else {
            builder.AddContent(60, Icon);
        }

        builder.CloseElement();
    }

    protected override void OnParametersSet() {
        base.OnParametersSet();
        if (Icon is null && ChildContent is null) {
            throw new InvalidOperationException("Must provide either child content or use a constructor which takes a string.");
        }
    }
}

