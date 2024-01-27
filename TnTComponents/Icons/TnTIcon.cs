using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents;
public abstract class TnTIcon {
    [Parameter]
    public MaterialIconSize Size { get; set; } = MaterialIconSize.Medium;

    [Parameter]
    public MaterialIconAppearance Appearance { get; set; } = MaterialIconAppearance.Default;

    [Parameter]
    public TnTColor Color { get; set; } = TnTColor.OnSurface;

    public string? AdditionalClass { get; set; }

    protected abstract string GetClass();

    public abstract MarkupString Render();
}

public enum MaterialIconSize {
    Small,
    Medium,
    Large,
    ExtraLarge
}

public enum MaterialIconAppearance {
    Default,
    Outlined,
    TwoTone,
    Round,
    Sharp
}