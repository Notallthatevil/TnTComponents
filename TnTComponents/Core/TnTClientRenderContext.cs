using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Core;
/// <inheritdoc/>
public sealed class TnTClientRenderContext : ITnTRenderContext {
    /// <inheritdoc/>
    public bool IsClient => true;

    /// <inheritdoc/>
    public bool IsServer => false;

    /// <inheritdoc/>
    public bool IsPrerendering => false;
}

