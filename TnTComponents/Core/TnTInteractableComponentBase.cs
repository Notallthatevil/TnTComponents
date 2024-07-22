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
}

