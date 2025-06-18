using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace TnTComponents;

/// <summary>
///     Represents the theme design component for TnT.
/// </summary>
public partial class TnTMeasurements
{
    /// <summary>
    /// Gets or sets the footer height.
    /// </summary>
    [Parameter]
    public double FooterHeight { get; set; } = 64;

    /// <summary>
    /// Gets or sets the header height.
    /// </summary>
    [Parameter]
    public double HeaderHeight { get; set; } = 64;

    /// <summary>
    /// Gets or sets the side navigation width.
    /// </summary>
    [Parameter]
    public double SideNavWidth { get; set; } = 256;

    /// <inheritdoc />
    public override string? ElementClass => throw new NotImplementedException();
    /// <inheritdoc />
    public override string? ElementStyle => throw new NotImplementedException();
    /// <inheritdoc />
    public override string? JsModulePath => "./_content/TnTComponents/Theming/TnTMeasurements.razor.js";

}