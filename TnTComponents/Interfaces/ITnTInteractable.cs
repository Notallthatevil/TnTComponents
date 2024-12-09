using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Interfaces;

/// <summary>
///     Represents an interactable component.
/// </summary>
public interface ITnTInteractable : ITnTComponentBase {

    /// <summary>
    ///     Gets or sets a value indicating whether the component is disabled.
    /// </summary>
    bool Disabled { get; }

    /// <summary>
    ///     Gets the name of the component.
    /// </summary>
    string? ElementName { get; }

    /// <summary>
    ///     Gets a value indicating whether the ripple effect is enabled.
    /// </summary>
    bool EnableRipple { get; }

    /// <summary>
    ///     Gets the on-tint color of the component.
    /// </summary>
    TnTColor? OnTintColor { get; }

    /// <summary>
    ///     Gets the tint color of the component.
    /// </summary>
    TnTColor? TintColor { get; }
}
