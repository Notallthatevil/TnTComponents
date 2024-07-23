using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Interfaces;
/// <summary>
/// Represents an interactable component.
/// </summary>
public interface ITnTInteractable {
    /// <summary>
    /// Gets or sets a value indicating whether the component is disabled.
    /// </summary>
    bool Disabled { get; }

    /// <summary>
    /// Gets the name of the component.
    /// </summary>
    string? Name { get; }

    bool EnableRipple { get; }
    TnTColor? TintColor { get; }
}

