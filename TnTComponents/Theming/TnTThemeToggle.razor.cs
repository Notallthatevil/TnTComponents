using TnTComponents.Core;

namespace TnTComponents;

/// <summary>
/// Represents a theme toggle component for switching between different themes.
/// </summary>
public partial class TnTThemeToggle {
    /// <inheritdoc />
    public override string? ElementClass => string.Empty;

    /// <inheritdoc />
    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    /// <inheritdoc />
    public override string? JsModulePath => "./_content/TnTComponents/Theming/TnTThemeToggle.razor.js";
}
