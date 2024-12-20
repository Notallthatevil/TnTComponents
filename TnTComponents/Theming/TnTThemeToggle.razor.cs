using TnTComponents.Core;

namespace TnTComponents;

/// <summary>
/// Represents a theme toggle component for switching between different themes.
/// </summary>
public partial class TnTThemeToggle {
    public override string? ElementClass => string.Empty;

    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    public override string? JsModulePath => "./_content/TnTComponents/Theming/TnTThemeToggle.razor.js";
}
