using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Interfaces;

namespace TnTComponents.Core;
public abstract class TnTInteractableComponentBase : TnTComponentBase, ITnTInteractable {
    [Parameter]
    // <inheritdoc/>
    public bool Disabled { get; set; }

    [Parameter]
    public string? Name { get; set; }
    [Parameter]
    public bool EnableRipple { get; set; } = true;
    [Parameter]
    public virtual TnTColor? TintColor { get; set; }

    [Parameter]
    public virtual TnTColor? OnTintColor { get; set; }
}

