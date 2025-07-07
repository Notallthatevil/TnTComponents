using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Core;
using TnTComponents.Layout;

namespace TnTComponents;

/// <summary>
/// Represents the body of a TnT layout component, typically used to contain the main content area.
/// </summary>
public partial class TnTBody {
    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create(base.ElementClass!)
        .AddClass("tnt-body")
        .Build();

}