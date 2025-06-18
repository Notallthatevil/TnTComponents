using Microsoft.AspNetCore.Components;
using TnTComponents.Core;

namespace TnTComponents;

/// <summary>
/// Represents a theme toggle component for switching between different themes.
/// </summary>
public partial class TnTThemeToggle {

    /// <summary>
    /// The default theme to use if no user preference is set.
    /// </summary>
    [Parameter]
    public Theme DefaultTheme { get; set; } = Theme.System;

    /// <summary>
    /// If true, allows the user to select a theme (Light, Dark, System).
    /// </summary>
    [Parameter]
    public bool AllowThemeSelection { get; set; } = true;

    /// <summary>
    /// If true, allows the user to select a contrast level (Default, Medium, High).
    /// </summary>
    [Parameter]
    public bool AllowContrastSelection { get; set; } = true;

    /// <summary>
    /// The root path for theme CSS files.
    /// </summary>
    [Parameter]
    public string ThemesRoot { get; set; } = "/Themes";

    // Theme CSS file names (defaults)
    /// <summary>
    /// The CSS file name for the light theme with default contrast.
    /// </summary>
    [Parameter]
    public string LightDefaultCss { get; set; } = "light.css";

    /// <summary>
    /// The CSS file name for the light theme with medium contrast.
    /// </summary>
    [Parameter]
    public string LightMediumCss { get; set; } = "light-mc.css";

    /// <summary>
    /// The CSS file name for the light theme with high contrast.
    /// </summary>
    [Parameter]
    public string LightHighCss { get; set; } = "light-hc.css";

    /// <summary>
    /// The CSS file name for the dark theme with default contrast.
    /// </summary>
    [Parameter]
    public string DarkDefaultCss { get; set; } = "dark.css";

    /// <summary>
    /// The CSS file name for the dark theme with medium contrast.
    /// </summary>
    [Parameter]
    public string DarkMediumCss { get; set; } = "dark-mc.css";

    /// <summary>
    /// The CSS file name for the dark theme with high contrast.
    /// </summary>
    [Parameter]
    public string DarkHighCss { get; set; } = "dark-hc.css";

    /// <inheritdoc />
    public override string? ElementClass => string.Empty;

    /// <inheritdoc />
    public override string? ElementStyle => string.Empty;

    /// <inheritdoc />
    public override string? JsModulePath => "./_content/TnTComponents/Theming/TnTThemeToggle.razor.js";
}
