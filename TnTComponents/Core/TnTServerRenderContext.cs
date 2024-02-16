using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Core;
/// <inheritdoc/>
public sealed class TnTServerRenderContext(IHttpContextAccessor contextAccessor) : ITnTRenderContext {
    /// <inheritdoc/>
    public bool IsClient => false;

    /// <inheritdoc/>
    public bool IsServer => true;

    /// <inheritdoc/>
    public bool IsPrerendering => !contextAccessor.HttpContext?.Response.HasStarted ?? false;
}
