using Microsoft.AspNetCore.Components;

namespace TnTComponents;

public enum MaterialIconSize {
    Small,
    Medium,
    Large,
    ExtraLarge
}

public abstract class TnTIcon {
    public string? AdditionalClass { get; set; }

    [Parameter]
    public MaterialIconAppearance Appearance { get; set; } = MaterialIconAppearance.Default;

    [Parameter]
    public TnTColor Color { get; set; } = TnTColor.OnSurface;

    [Parameter]
    public MaterialIconSize Size { get; set; } = MaterialIconSize.Medium;

    public abstract MarkupString Render();

    protected abstract string GetClass();
}

public enum MaterialIconAppearance {
    Default,
    Outlined,
    TwoTone,
    Round,
    Sharp
}